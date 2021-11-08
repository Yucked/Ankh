using Ankh.Data;
using Marten;
using Marten.Linq;

namespace Ankh {
    public sealed class Repository<T>
        where T : struct, IData {
        private readonly IDocumentStore _documentStore;
        private readonly string _tableName;

        public Repository(IDocumentStore documentStore) {
            _documentStore = documentStore;
            if (typeof(T) == typeof(UserData)) {
                _tableName = "mt_doc_userdata";
            }

            if (typeof(T) == typeof(RoomData)) {
                _tableName = "mt_doc_roomdata";
            }

            if (typeof(T) == typeof(DirectoryData)) {
                _tableName = "mt_doc_directorydata";
            }
        }

        public async Task InsertAsync(T value) {
            await using var session = _documentStore.LightweightSession();
            if (await ExistsAsync(value.Id)) {
                return;
            }

            session.Insert(value);
            await session.SaveChangesAsync();
        }

        public async Task InsertOrUpdateAsync(T value) {
            await using var session = _documentStore.LightweightSession();
            if (await ExistsAsync(value.Id)) {
                await UpdateAsync(value, session);
                return;
            }

            session.Insert(value);
            await session.SaveChangesAsync();
        }

        public Task DeleteAsync(string key) {
            using var session = _documentStore.LightweightSession();
            session.Delete<T>(key);
            return session.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string key) {
            await using var session = _documentStore.QuerySession();
            var queryResult = await session.QueryAsync<bool>
                ($"SELECT EXISTS(SELECT id FROM {_tableName} WHERE id='{key}')");
            return queryResult[0];
        }

        public Task<T> GetAsync(string key) {
            using var session = _documentStore.LightweightSession();
            return session.LoadAsync<T>(key);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync() {
            await using var session = _documentStore.QuerySession();
            return await session.QueryAsync<T>($"SELECT * FROM {_tableName}");
        }

        public async Task UpdateAsync(T value, IDocumentSession documentSession = null) {
            await using var session = documentSession ?? _documentStore.LightweightSession();
            var oldValue = await session.LoadAsync<T>(value.Id);
            session.Update(oldValue.Update(value));
            await session.SaveChangesAsync();
        }

        public async ValueTask<int> CountAsync() {
            await using var session = _documentStore.QuerySession();
            var queryResult = await session.QueryAsync<int>($"SELECT COUNT(id) FROM {_tableName}");
            return queryResult[0];
        }
    }
}
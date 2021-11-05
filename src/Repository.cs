using Ankh.Data;
using Marten;

namespace Ankh {
    public sealed class Repository<T>
        where T : struct, IData {
        private readonly IDocumentStore _documentStore;

        public Repository(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public async Task InsertAsync(T value) {
            await using var session = _documentStore.LightweightSession();
            if (await ExistsAsync(value.Id, session)) {
                return;
            }

            session.Insert(value);
            await session.SaveChangesAsync();
        }

        public async Task InsertOrUpdateAsync(T value) {
            await using var session = _documentStore.LightweightSession();
            if (await ExistsAsync(value.Id, session)) {
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

        public Task<bool> ExistsAsync(string key, IDocumentSession documentSession) {
            return documentSession.Query<T>().AnyAsync(x => x.Id == key);
        }

        public Task<T> GetAsync(string key) {
            using var session = _documentStore.LightweightSession();
            return session.LoadAsync<T>(key);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync() {
            await using var session = _documentStore.LightweightSession();
            return await session.Query<T>().ToListAsync();
        }

        public async Task UpdateAsync(T value, IDocumentSession documentSession = null) {
            await using var session = documentSession ?? _documentStore.LightweightSession();
            var oldValue = await session.LoadAsync<T>(value.Id);
            session.Update(oldValue.Update(value));
            await session.SaveChangesAsync();
        }
    }
}
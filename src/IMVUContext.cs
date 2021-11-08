using Ankh.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Ankh {
    public class IMVUContext : DbContext {
        public DbSet<UserData> Users { get; set; }
        public DbSet<RoomData> Rooms { get; set; }
        public DbSet<DirectoryData> Directory { get; set; }

        public IMVUContext(DbContextOptions<IMVUContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<RoomData>(builder => {
                builder.Property(x => x.UserHistory)
                .HasConversion(
                    s => JsonSerializer.Serialize(s, typeof(Dictionary<string, DateTimeOffset>), default(JsonSerializerOptions)),
                    d => JsonSerializer.Deserialize<IDictionary<string, DateTimeOffset>>(d, default(JsonSerializerOptions)));
            });

            modelBuilder.Entity<UserData>()
                .HasOne(x => x.Picture);

            base.OnModelCreating(modelBuilder);
        }
    }
}

namespace Ankh.Tests;
 
[TestClass]
public sealed class RoomHandlerTests {
    [DataTestMethod]
    [DataRow()]
    public async Task Test_GetRoomByIdAsync(int userId, int roomId) {
        //Globals.RoomHandler.GetRoomByIdAsync(userId, roomId);
    }
    
    [DataTestMethod]
    [DataRow("Foo", "Bar", "Query")]
    public async Task Test_SearchRoomsAsync(string u, string p, string kw) {
        var userSauce = await Globals.UserHandler.LoginAsync(u, p);
        Assert.IsNotNull(userSauce);
        Assert.IsNotNull(userSauce.Auth);
        
        await Globals.RoomHandler.SearchRoomsAsync(userSauce, q => {
            q.Keywords = kw;
        });
    }
}
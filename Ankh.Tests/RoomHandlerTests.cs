namespace Ankh.Tests;

[TestClass]
public sealed class RoomHandlerTests {
    [DataTestMethod]
    [DataRow("116678358-646")]
    [DataRow("263636495-130")]
    [DataRow("347951586-161")]
    public async Task Test_GetRoomByIdAsync(string roomId) {
        var userLogin = await Globals.UserHandler.LoginAsync(Globals.DummyLogin.Username, Globals.DummyLogin.Password);
        var room = await Globals.RoomHandler.GetRoomByIdAsync(userLogin, roomId);
        Assert.IsNotNull(room.Name);
    }
    
    [DataTestMethod]
    [DataRow("Query")]
    public async Task Test_SearchRoomsAsync(string kw) {
        var userLogin = await Globals.UserHandler.LoginAsync(Globals.DummyLogin.Username, Globals.DummyLogin.Password);
        Assert.IsNotNull(userLogin);
        Assert.IsNotNull(userLogin.SessionId);
        
        await Globals.RoomHandler.SearchRoomsAsync(userLogin, q => { q.Keywords = kw; });
    }
    
    [DataTestMethod]
    [DataRow(347951586)]
    public async Task Test_GetPublicRoomsForUsersAsync(long userId) {
        var userLogin = await Globals.UserHandler.LoginAsync(Globals.DummyLogin.Username, Globals.DummyLogin.Password);
        var rooms = await Globals.RoomHandler.GetPublicRoomsForUsersAsync(userLogin, userId);
        Assert.IsNotNull(rooms);
    }
}
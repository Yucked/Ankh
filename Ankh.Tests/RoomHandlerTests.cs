namespace Ankh.Tests;
 
[TestClass]
public sealed class RoomHandlerTests {
    [DataTestMethod]
    [DataRow()]
    public async Task Test_GetRoomByIdAsync(int roomId) {
        Globals.RoomHandler.GetRoomByIdAsync(roomId);
    }
}
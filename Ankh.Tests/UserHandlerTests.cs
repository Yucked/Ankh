namespace Ankh.Tests;

[TestClass]
public sealed class UserHandlerTests {
    [DataTestMethod]
    [DataRow(347951586)]
    [DataRow(327222770)]
    [DataRow(350843753)]
    [DataRow(116678358)]
    [DataRow(121197925)]
    public async Task Test_GetUserByIdAsync(int userId) {
        var user = await Globals.UserHandler.GetUserByIdAsync(userId);
        Assert.IsNotNull(user);
        Assert.IsNotNull(user.Username);
    }
    
    [DataTestMethod]
    [DataRow(347951586)]
    [DataRow(327222770)]
    [DataRow(350843753)]
    [DataRow(116678358)]
    [DataRow(121197925)]
    public async Task Test_GetUserProfileAsync(int userId) {
        var user = await Globals.UserHandler.GetUserProfileAsync(userId);
        Assert.IsNotNull(user);
        Assert.IsNotNull(user.Username);
    }
    
    [DataTestMethod]
    [DataRow(347951586, 327222770, 350843753, 116678358, 121197925)]
    public async Task Test_GetUsersByIdAsync(params int[] userIds) {
        var userSauce = await Globals.UserHandler.LoginAsync(Globals.DummyLogin.Username, Globals.DummyLogin.Password);
        Assert.IsNotNull(userSauce);
        Assert.IsNotNull(userSauce.Auth);
        
        var users = await Globals.UserHandler.GetUsersByIdAsync(userSauce, userIds);
        Assert.IsNotNull(users);
        Assert.IsTrue(users.Count > 0);
    }
    
    [DataTestMethod]
    [DataRow("Liep")]
    [DataRow("zimm")]
    [DataRow("veza")]
    [DataRow("duMaison")]
    [DataRow("Delly")]
    public async Task Test_GetIdFromUsernameAsync(string username) {
        var userId = await Globals.UserHandler.GetIdFromUsernameAsync(username);
        Assert.IsNotNull(userId);
    }
    
    [TestMethod]
    public async Task Test_LoginAsync() {
        var userSauce = await Globals.UserHandler.LoginAsync(Globals.DummyLogin.Username, Globals.DummyLogin.Password);
        Assert.IsNotNull(userSauce);
        Assert.IsNotNull(userSauce.Auth);
    }
    
    [TestMethod]
    public async Task Test_GetUserOutfitsAsync() {
        var userSauce = await Globals.UserHandler.LoginAsync(Globals.DummyLogin.Username, Globals.DummyLogin.Password);
        Assert.IsNotNull(userSauce);
        Assert.IsNotNull(userSauce.Auth);
        
        var outfits = await Globals.UserHandler.GetUserOutfitsAsync(userSauce);
        Assert.IsNotNull(outfits);
        Assert.IsTrue(outfits.Length > 0);
    }
}
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
    [DataRow("Foo", "Bar", 347951586, 327222770, 350843753, 116678358, 121197925)]
    public async Task Test_GetUsersByIdAsync(string username, string password, params int[] userIds) {
        var userSauce = await Globals.UserHandler.LoginAsync(username, password);
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
    
    [DataTestMethod]
    [DataRow("Foo", "Bar")]
    public async Task Test_LoginAsync(string username, string password) {
        var userSauce = await Globals.UserHandler.LoginAsync(username, password);
        Assert.IsNotNull(userSauce);
        Assert.IsNotNull(userSauce.Auth);
        
        var result = Saucery.TryStore(username, userSauce);
        Assert.IsTrue(result);
    }
    
    [DataTestMethod]
    [DataRow("Foo", "Bar")]
    public async Task Test_GetUserOutfitsAsync(string username, string password) {
        var userSauce = await Globals.UserHandler.LoginAsync(username, password);
        Assert.IsNotNull(userSauce);
        Assert.IsNotNull(userSauce.Auth);
        
        var outfits = await Globals.UserHandler.GetUserOutfitsAsync(userSauce);
        Assert.IsNotNull(outfits);
        Assert.IsTrue(outfits.Length > 0);
    }
}
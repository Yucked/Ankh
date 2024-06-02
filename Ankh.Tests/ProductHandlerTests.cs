namespace Ankh.Tests;

[TestClass]
public class ProductHandlerTests {
    [DataTestMethod]
    [DataRow(
        "https://www.imvu.com/catalog/products_in_scene.php?avatar138341669=21037123%3B22982265%3B25165372%3B35334309%3B49250865&avatar116678358=12378884%3B13683015%3B13848680%3B17159540%3B22970422%3B43584093%3B44583827%3B46918879%3B51537659%3B60917503%3B61191059%3B62077072%3B62307466%3B64225123%3B65012503%3B65198072%3B65520913%3B65545877%3B67005143&room=3376274&avatar149345959=35172561&avatar113728166=15144595%3B16075955%3B18105562%3B59997875&avatar61175063=4655556%3B5640080%3B10270736%3B12453495%3B13683313%3B18366100%3B20125329%3B21610559%3B22278660%3B24892464%3B29711833%3B30541011%3B33456531%3B36197919%3B37344923%3B48076326%3B55278075%3B58288163%3B59665903%3B60684309%3B62133386%3B63286926%3B63431904%3B63434764%3B65174054%3B66082007%3B66090003%3B66090068%3B66894737%3B67384610&avatar27269599=457281%3B12432136%3B12493914%3B16070331%3B20389259%3B22278641%3B25043858%3B31316879%3B33180207%3B33180241%3B33323581%3B33521828%3B33690025%3B33711381%3B33730513%3B33736891%3B33752880%3B34138620%3B34442570%3B40455510%3B40696805%3B40852756%3B43076466%3B45294902%3B45320661%3B45724661%3B49974077%3B51338981%3B51453658%3B51510974%3B53538726%3B55270542%3B56746347%3B60062564%3B60067199%3B60125608%3B60406527%3B62226029%3B63605940%3B65071048%3B67256772%3B67308173%3B67599095%3B67654834%3B67673044&avatar296956860=39159426%3B42908118%3B44334707%3B44779587%3B45344974%3B46796325%3B48608231%3B50178051%3B53445775%3B55265587%3B55885730%3B57026303%3B58278125%3B60073061%3B60178921%3B60345714%3B61604646%3B63097916%3B63972735%3B64187867%3B64433823%3B64676124%3B64801300%3B64817773%3B65410987%3B66616921&avatar106931187=16070361%3B28532919%3B38269864%3B46218746%3B47089963%3B49507988%3B49510878%3B52994665%3B53442346%3B57700117%3B59017636%3B59771276%3B60073061%3B62321061%3B63321592%3B63521358%3B63536218%3B65062777%3B65062998%3B65580767%3B66311539%3B67723474%3B67733066&avatar84166876=17730211%3B19335068%3B26755770%3B27130129%3B30115411%3B30541011%3B37609498%3B37704915%3B38987455%3B42559693%3B43076512%3B43076548%3B44418678%3B45921760%3B45921939%3B46117897%3B48594957%3B52931639%3B53934681%3B59913761%3B64061925%3B66181170%3B66959309%3B67211033%3B67657732&avatar148721598=6070070%3B8597079%3B8996667%3B13683203%3B18734823%3B22278658%3B24790328%3B27308827%3B28643430%3B30526258%3B30844614%3B31218520%3B31770019%3B36857011%3B37802620%3B42286398%3B44284040%3B44833889%3B45521392%3B45521457%3B45521462%3B47599670%3B48957481%3B49375015%3B51909106%3B52207465%3B54485991%3B54985359%3B54997425%3B56098679%3B56256251%3B56456865%3B57644899%3B57915606%3B58164786%3B60518661%3B61433798%3B62648912%3B62750342%3B64282345%3B64376473%3B65923763%3B66169097%3B66351023")]
    [DataRow(
        "https://www.imvu.com/catalog/products_in_scene.php?avatar137287334=13848769%3B14469444%3B20883890%3B25332026%3B25966564%3B32290250%3B36634494%3B38145283%3B41398655%3B41973955%3B48105303%3B52664670%3B53051430%3B55088473%3B56732359%3B57720385%3B58546831%3B63101145%3B63628611%3B64054350%3B65002018%3B65174862%3B65179617%3B67768735%3B67774861%3B67774891%3B67774906&room=15505727x2%3B26449267x5%3B26856780%3B30417879%3B36106579%3B37071120x3%3B39070742%3B44232469%3B59650789%3B60286066%3B60312621%3B60325791%3B60325940%3B60369419%3B60373509%3B60735778x2%3B60844641%3B61887768%3B64613496%3B64618561%3B64740725%3B66155663%3B66892526&avatar357444795=12257097%3B13759128%3B22275280%3B28329882%3B30988733%3B39455287%3B41503955%3B41513590%3B43213545%3B45067805%3B54198804%3B55010633%3B62632214%3B62632223%3B63014450%3B64495967%3B64595665%3B64746692%3B65515798%3B65714264%3B65731247%3B65731502%3B65966340&avatar354783732=13759128%3B41429432%3B45226263%3B45282789%3B46549996%3B53661499%3B55845245%3B56355657%3B58217223%3B59672827%3B60883073%3B62015709%3B62058104%3B62637558%3B62931660%3B63108262%3B63157043%3B64758674%3B65012475%3B66780448%3B66831983%3B66956658%3B67047448%3B67116226%3B67560301&avatar286538590=13759128%3B28905562%3B33128658%3B37817958%3B50493679%3B51324741%3B51441438%3B51494422%3B51746633&avatar122221239=22278615%3B31104174%3B36770287%3B58029261%3B58401944%3B58991496%3B59294733%3B59365442%3B62067375%3B62157822%3B63790812%3B65083584%3B65337520%3B65552928%3B66658960%3B66887777%3B66968670%3B67434999%3B67671637%3B67732761&avatar371215864=13759128%3B45622089%3B45941410%3B50539332%3B50943054%3B51339279%3B52373195%3B53872143%3B55636690%3B55753196%3B58312529%3B58644054%3B58644077%3B58970316%3B59190649%3B60813213%3B62686573%3B65767874%3B66086108%3B66167914%3B66201017%3B66999381%3B67696141%3B67728656&avatar142712680=12441192%3B13683203%3B22278635%3B28348599%3B33061817%3B44957164%3B46831084%3B47978022%3B48873414%3B55404160%3B56340406%3B61124151%3B62382819%3B63294436%3B63363534%3B64016205%3B64080077%3B64308854%3B64594171%3B64683262%3B64912512%3B66272055%3B66957418%3B67412474%3B67474611&avatar299337992=4032675%3B5073071%3B8481180%3B12871507%3B13759128%3B15676905%3B17159540%3B17275075%3B21987069%3B27400160%3B35836426%3B39165697%3B39368110%3B40223646%3B43862792%3B45223479%3B45282693%3B45438955%3B46437174%3B48067694%3B48606985%3B49243544%3B50791759%3B50829088%3B50833412%3B50968155%3B51200641%3B52605074%3B53164370%3B53808850%3B54186828%3B54774888%3B55352175%3B55503493%3B55764610%3B56503351%3B56962424%3B57365363%3B57560944%3B58646345&avatar116678358=12378884%3B13848680%3B20330137%3B29836120%3B43825792%3B48851409%3B56463187%3B58577713%3B60448849%3B60846287%3B61452185%3B61733936%3B62613925%3B63347464%3B63348342%3B64433221%3B65520913%3B67080364%3B67318209%3B67352475")]
    public async Task Test_GetProductsInSceneAsync(string url) {
        var products = await Globals.ProductHandler.GetProductsInSceneAsync(url);
        Assert.IsNotNull(products);
        Assert.IsTrue(products.Count > 0);
    }
}
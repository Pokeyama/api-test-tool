using DFrame;

namespace Rabbit;


/// <summary>
/// 郵便番号検索API
/// </summary>
[Workload("郵便番号検索API")]
public class SearchPostalCode : Workload
{
    private readonly HttpClient _httpClient;
    private readonly string _postalCode;
    private string url;
    private string resultJson;

    /// <summary>
    /// コンストラクタ
    /// コンストラクタに変数を追加するとGUI上から取得できます。
    /// </summary>
    /// <param name="httpClient">httpClient</param>
    /// <param name="postalCode">郵便番号</param>
    public SearchPostalCode(HttpClient httpClient, string postalCode)
    {
        this._httpClient = httpClient;
        this._postalCode = postalCode;
    }

    /// <summary>
    /// 前準備
    /// </summary>
    /// <param name="context"></param>
    public override async Task SetupAsync(WorkloadContext context)
    {
        var parameter = new Dictionary<string, string>()
        {
            { "zipcode", this._postalCode }
        };
        this.url =
            $"https://zipcloud.ibsnet.co.jp/api/search?{await new FormUrlEncodedContent(parameter).ReadAsStringAsync()}";
    }
    
    /// <summary>
    /// 実行
    /// </summary>
    /// <param name="context"></param>
    public override async Task ExecuteAsync(WorkloadContext context)
    {
        var response = await _httpClient.GetAsync(this.url, context.CancellationToken);
        
        this.resultJson = response.Content.ReadAsStringAsync().Result;
    }
    
    /// <summary>
    /// 後始末
    /// </summary>
    /// <param name="context"></param>
    public override async Task TeardownAsync(WorkloadContext context)
    {
        
    }
    
    /// <summary>
    /// すべて終了後に呼び出される
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Dictionary<string, string>? Complete(WorkloadContext context)
    {
        // 連想配列でGUI側に値を出力できます。
        return new ()
        {
            {"result", resultJson},
        };
    }
}

/// <summary>
/// 郵便番号検索API
/// </summary>
[Workload("URLを埋め込んでUnitTestみたいにする")]
public class SearchPostalCodeAnet : Workload
{
    private readonly HttpClient _httpClient;
    private string? resultJson;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="httpClient"></param>
    public SearchPostalCodeAnet(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// 前準備
    /// </summary>
    /// <param name="context"></param>
    public override async Task SetupAsync(WorkloadContext context)
    {
        
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <param name="context"></param>
    public override async Task ExecuteAsync(WorkloadContext context)
    {
        var response = await _httpClient.GetAsync("https://zipcloud.ibsnet.co.jp/api/search?zipcode=171-0022");

        this.resultJson = response.Content.ReadAsStringAsync().Result;
    }
    
    /// <summary>
    /// 後始末
    /// </summary>
    /// <param name="context"></param>
    public override async Task TeardownAsync(WorkloadContext context)
    {
        
    }
    
    /// <summary>
    /// すべて終了後に呼び出される
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Dictionary<string, string>? Complete(WorkloadContext context)
    {
        // 連想配列でGUI側に値を出力できます。
        return new ()
        {
            {"result", resultJson},
        };
    }
}

using System.Net.Http.Json;

public class ZelenkaAPI
{
    public static string apiKey { get; set; }

    public static async Task<ZelenkaListJson> GetListAccouns()
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync("https://api.lzt.market/telegram?pmax=40&scam=no&spam=no&password=no&premium=no&not_country%5B%5D=RU&daybreak=1&min_channels=5");
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
            ZelenkaListJson content = await response.Content.ReadFromJsonAsync<ZelenkaListJson>();
            
            return content;
        }
    }
    public static async Task<ZelenkaBuyJson> BuyTGaccount(int price, int itemId)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync($"https://api.lzt.market/{itemId}/fast-buy?price={price}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            ZelenkaBuyJson content = await response.Content.ReadFromJsonAsync<ZelenkaBuyJson>();
            return content;
        }
    }

    public static async Task<ZelenkaTGCode> GetCode(int itemId)
    {
        using (HttpClient client = new HttpClient())
        {

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync($"https://api.lzt.market/{itemId}/telegram-login-code");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            ZelenkaTGCode content = await response.Content.ReadFromJsonAsync<ZelenkaTGCode>();
            return content;
        }
    }

}




public class ZelenkaListJson
{
    public Item[] items { get; set; }
    public int totalItems { get; set; }
    public object totalItemsPrice { get; set; }
    public int perPage { get; set; }
    public int page { get; set; }
    public int cacheTTL { get; set; }
    public int lastModified { get; set; }
    public string searchUrl { get; set; }
    public object[] stickyItems { get; set; }
    public bool isIsolatedMarket { get; set; }
    public bool isIsolatedMarketAlt { get; set; }
    public System_Info system_info { get; set; }
}
public class System_Info
{
    public int visitor_id { get; set; }
    public int time { get; set; }
}
public class Item
{
    public int item_id { get; set; }
    public string item_state { get; set; }
    public int category_id { get; set; }
    public int published_date { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int update_stat_date { get; set; }
    public int refreshed_date { get; set; }
    public int view_count { get; set; }
    public int is_sticky { get; set; }
    public string item_origin { get; set; }
    public int extended_guarantee { get; set; }
    public int nsb { get; set; }
    public int allow_ask_discount { get; set; }
    public string title_en { get; set; }
    public string description_en { get; set; }
    public string email_type { get; set; }
    public int is_reserved { get; set; }
    public string item_domain { get; set; }
    public int active_auction { get; set; }
    public int telegram_item_id { get; set; }
    public string telegram_country { get; set; }
    public int telegram_last_seen { get; set; }
    public int telegram_scam { get; set; }
    public object telegram_verified { get; set; }
    public int telegram_premium { get; set; }
    public int telegram_password { get; set; }
    public int telegram_premium_expires { get; set; }
    public int telegram_spam_block { get; set; }
    public int telegram_channels_count { get; set; }
    public int telegram_chats_count { get; set; }
    public int telegram_admin_count { get; set; }
    public int telegram_admin_subs_count { get; set; }
    public int telegram_conversations_count { get; set; }
    public int telegram_id_count { get; set; }
    public bool isIgnored { get; set; }
    public bool canViewLoginData { get; set; }
    public bool canUpdateItemStats { get; set; }
    public bool showGetEmailCodeButton { get; set; }
    public bool canOpenItem { get; set; }
    public bool canCloseItem { get; set; }
    public bool canEditItem { get; set; }
    public bool canDeleteItem { get; set; }
    public bool canStickItem { get; set; }
    public bool canUnstickItem { get; set; }
    public Bumpsettings bumpSettings { get; set; }
    public bool canBumpItem { get; set; }
    public bool canBuyItem { get; set; }
    public int rub_price { get; set; }
    public string price_currency { get; set; }
    public bool canValidateAccount { get; set; }
    public bool canResellItemAfterPurchase { get; set; }
    public Telegram_Group_Counters telegram_group_counters { get; set; }
    public Telegram_Admin_Groups[] telegram_admin_groups { get; set; }
    public bool canViewAccountLink { get; set; }
    public string itemOriginPhrase { get; set; }
    public object[] tags { get; set; }
    public object note_text { get; set; }
    public object[] auction { get; set; }
    public Reserve reserve { get; set; }
    public string description_html { get; set; }
    public string description_html_en { get; set; }
    public Seller seller { get; set; }
    public int sold_items_category_count { get; set; }
    public int restore_items_category_count { get; set; }
    public bool telegram_spam_block_expired { get; set; }
}
public class Bumpsettings
{
    public bool canBumpItem { get; set; }
    public bool canBumpItemGlobally { get; set; }
    public object shortErrorPhrase { get; set; }
    public object errorPhrase { get; set; }
}
public class Telegram_Group_Counters
{
    public int chats { get; set; }
    public int channels { get; set; }
    public int conversations { get; set; }
    public int admin { get; set; }
}
public class Reserve
{
    public int reserve_user_id { get; set; }
    public int reserve_date { get; set; }
}
public class Seller
{
    public int user_id { get; set; }
    public int sold_items_count { get; set; }
    public string restore_data { get; set; }
    public string username { get; set; }
    public int avatar_date { get; set; }
    public int is_banned { get; set; }
    public int display_style_group_id { get; set; }
    public int? restore_percents { get; set; }
}
public class Telegram_Admin_Groups
{
    public string title { get; set; }
    public long id { get; set; }
    public int participants_count { get; set; }
    public string username { get; set; }
    public bool verified { get; set; }
    public bool owner { get; set; }
}









public class ZelenkaBuyJson
{
    public string status { get; set; }
    public ItemBuy item { get; set; }
    public bool isIsolatedMarket { get; set; }
    public bool isIsolatedMarketAlt { get; set; }
    public System_Info system_info { get; set; }
}
public class ItemBuy
{
    public int item_id { get; set; }
    public string item_state { get; set; }
    public int category_id { get; set; }
    public int published_date { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int update_stat_date { get; set; }
    public int refreshed_date { get; set; }
    public string login { get; set; }
    public string temp_email { get; set; }
    public int view_count { get; set; }
    public int is_sticky { get; set; }
    public string information { get; set; }
    public string item_origin { get; set; }
    public int extended_guarantee { get; set; }
    public int nsb { get; set; }
    public int allow_ask_discount { get; set; }
    public string title_en { get; set; }
    public string description_en { get; set; }
    public string email_type { get; set; }
    public int is_reserved { get; set; }
    public string item_domain { get; set; }
    public int active_auction { get; set; }
    public int user_allow_ask_discount { get; set; }
    public int max_discount_percent { get; set; }
    public string market_custom_title { get; set; }
    public int buyer_avatar_date { get; set; }
    public int buyer_user_group_id { get; set; }
    public string buyer_secondary_group_ids { get; set; }
    public int telegram_item_id { get; set; }
    public long telegram_id { get; set; }
    public string telegram_phone { get; set; }
    public string telegram_country { get; set; }
    public int telegram_last_seen { get; set; }
    public string telegram_username { get; set; }
    public int telegram_scam { get; set; }
    public object telegram_verified { get; set; }
    public int telegram_premium { get; set; }
    public int telegram_password { get; set; }
    public int telegram_premium_expires { get; set; }
    public int telegram_spam_block { get; set; }
    public int telegram_channels_count { get; set; }
    public int telegram_chats_count { get; set; }
    public int telegram_admin_count { get; set; }
    public int telegram_admin_subs_count { get; set; }
    public int telegram_conversations_count { get; set; }
    public int telegram_id_count { get; set; }
    public bool canViewLoginData { get; set; }
    public bool canUpdateItemStats { get; set; }
    public Logindata loginData { get; set; }
    public bool showGetEmailCodeButton { get; set; }
    public bool showGetTelegramCodeButton { get; set; }
    public object getEmailCodeDisplayLogin { get; set; }
    public Buyer buyer { get; set; }
    public bool isPersonalAccount { get; set; }
    public int rub_price { get; set; }
    public string price_currency { get; set; }
    public bool canValidateAccount { get; set; }
    public bool canResellItemAfterPurchase { get; set; }
    public Telegram_Group_Counters telegram_group_counters { get; set; }
    public Telegram_Admin_Groups[] telegram_admin_groups { get; set; }
    public bool canViewAccountLink { get; set; }
    public object accountLink { get; set; }
    public string itemOriginPhrase { get; set; }
    public bool visitorIsAuthor { get; set; }
    public bool canAskDiscount { get; set; }
    public bool canCheckGuarantee { get; set; }
    public Tags tags { get; set; }
    public object[] customFields { get; set; }
    public object[] externalAuth { get; set; }
    public bool isTrusted { get; set; }
    public bool isIgnored { get; set; }
    public int deposit { get; set; }
    public Extraprice[] extraPrices { get; set; }
    public bool canViewAccountLoginAndTempEmail { get; set; }
    public Bumpsettings bumpSettings { get; set; }
    public object auction { get; set; }
    public Reserve reserve { get; set; }
    public string description_html { get; set; }
    public string description_html_en { get; set; }
    public SellerBuy seller { get; set; }
}
public class Logindata
{
    public string raw { get; set; }
    public string encodedRaw { get; set; }
    public string login { get; set; }
    public string password { get; set; }
    public string encodedPassword { get; set; }
    public string oldPassword { get; set; }
    public object encodedOldPassword { get; set; }
}
public class Buyer
{
    public int user_id { get; set; }
    public int operation_date { get; set; }
    public bool visitorIsBuyer { get; set; }
    public string username { get; set; }
    public int is_banned { get; set; }
    public int display_style_group_id { get; set; }
    public string uniq_username_css { get; set; }
    public string secondary_group_ids { get; set; }
    public int user_group_id { get; set; }
}
public class Tags
{
    public _1 _1 { get; set; }
}
public class _1
{
    public int tag_id { get; set; }
    public string title { get; set; }
    public bool isDefault { get; set; }
    public bool forOwnedAccountsOnly { get; set; }
    public string bc { get; set; }
}
public class SellerBuy
{
    public int user_id { get; set; }
    public string username { get; set; }
    public int avatar_date { get; set; }
    public int is_banned { get; set; }
    public int display_style_group_id { get; set; }
    public int joined_date { get; set; }
    public int sold_items_count { get; set; }
    public int active_items_count { get; set; }
    public string restore_data { get; set; }
    public object restore_percents { get; set; }
}
public class Extraprice
{
    public string currency { get; set; }
    public float price { get; set; }
}





public class ZelenkaTGCode
{
    public ItemTgCode item { get; set; }
    public Code[] codes { get; set; }
    public bool isIsolatedMarket { get; set; }
    public bool isIsolatedMarketAlt { get; set; }
    public System_Info system_info { get; set; }
}
public class ItemTgCode
{
    public int item_id { get; set; }
    public string item_state { get; set; }
    public int category_id { get; set; }
    public int published_date { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int update_stat_date { get; set; }
    public int refreshed_date { get; set; }
    public int view_count { get; set; }
    public int is_sticky { get; set; }
    public string item_origin { get; set; }
    public int extended_guarantee { get; set; }
    public int nsb { get; set; }
    public int allow_ask_discount { get; set; }
    public string title_en { get; set; }
    public string description_en { get; set; }
    public string email_type { get; set; }
    public int is_reserved { get; set; }
    public string item_domain { get; set; }
    public int active_auction { get; set; }
    public int telegram_item_id { get; set; }
    public long telegram_id { get; set; }
    public string telegram_phone { get; set; }
    public string telegram_country { get; set; }
    public int telegram_last_seen { get; set; }
    public string telegram_username { get; set; }
    public int telegram_scam { get; set; }
    public object telegram_verified { get; set; }
    public int telegram_premium { get; set; }
    public int telegram_password { get; set; }
    public int telegram_premium_expires { get; set; }
    public string telegram_groups { get; set; }
    public int telegram_spam_block { get; set; }
    public string telegram_formatted_phone { get; set; }
    public int telegram_channels_count { get; set; }
    public int telegram_chats_count { get; set; }
    public int telegram_admin_count { get; set; }
    public int telegram_admin_subs_count { get; set; }
    public int telegram_conversations_count { get; set; }
    public int telegram_id_count { get; set; }
    public Reserve reserve { get; set; }
    public string description_html { get; set; }
    public string description_html_en { get; set; }
    public Seller seller { get; set; }
}
public class Code
{
    public string code { get; set; }
    public int date { get; set; }
}

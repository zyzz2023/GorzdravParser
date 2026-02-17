using GorzdravParser.Core.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using OpenQA.Selenium.Support.UI;

public class HtmlLoader
{
    private readonly string _url;
    private readonly ChromeOptions _options;
    private readonly IParserSettings _settings;

    public HtmlLoader(IParserSettings settings)
    {
        _settings = settings;
        _url = $"{_settings.Url}/";

        _options = new ChromeOptions();
        _options.AddArgument("--headless"); // без GUI
        _options.AddArgument("--disable-gpu");
        _options.AddArgument("--lang=ru");
        _options.AddArgument("--no-sandbox");
    }

    public string GetSourceByCurrentPage(int currentPage)
    {
        try
        {
            Console.WriteLine($"Получение данных со страницы {currentPage}...");

            using var driver = new ChromeDriver(_options);

            if(currentPage == 1)
            {
                driver.Navigate().GoToUrl(_url);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(s => s.FindElements(By.CssSelector(".product-card")).Count >= _settings.CountProducts);
            }
            else
            {
                driver.Navigate().GoToUrl(_url);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(s => s.FindElements(By.CssSelector(".ui-table-pagination__arrow-container")).Count > 0);

                for(int i = 1; i <= currentPage; i++)
                {
                    CloseCookie(driver);

                    try
                    {
                        var nextButton = wait.Until(x =>
                        {
                            var buttons = x.FindElements(By.CssSelector(".ui-table-pagination__arrow-container"));

                            return buttons.FirstOrDefault(b =>
                                !b.GetAttribute("class").Contains("disabled") &&
                                b.Displayed &&
                                b.Enabled);
                        });

                        nextButton.Click();

                        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                        wait.Until(s => s.FindElements(By.CssSelector(".product-card")).Count > 0);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при переходе на страницу {i + 1}: {ex.Message}");
                        throw;
                    }
                }
            }

            var source = driver.PageSource;

            return source;
        }
        catch(WebDriverException ex)
        {
            Console.WriteLine($"Ошибка при загрузке страницы {currentPage}: {ex.Message}");
            return string.Empty;
        }
    }

    private static void CloseCookie(ChromeDriver driver)
    {
        try
        {
            var cookieWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            cookieWait.Until(d => d.FindElement(By.CssSelector(".cookie-modal")).Displayed);

            var closeButton = driver.FindElement(By.CssSelector(".cookie-modal button, .cookie-modal .close"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", closeButton);
            Console.WriteLine("Cookie-окно закрыто");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Cookie-окно не появилось");
        }
    }
}

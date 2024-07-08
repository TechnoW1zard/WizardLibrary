using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;

namespace WizardLibrary.Appium
{
    /// <summary>
    /// Расширенные методы для класса AndroidElement. Можно использовать когда возвращается класс.
    /// </summary>
    public static class AndroidElementExtensions
    {
        /// <summary>
        /// Записывает в полученый элемент текст
        /// </summary>
        /// <param name="element">Нужно передать класс AndroidElement</param>
        /// <param name="text">Текст который вы хотите отправить</param>
        /// <param name="delay">Задержка после ввода текста. По умолчанию 2 с.</param>
        /// <returns>Возвращает элемент обратно</returns>
        public static AndroidElement SendKeysWithDelay(this AndroidElement element, string text, int delay = 2000)
        {
            element.SendKeys(text);
            Thread.Sleep(delay);
            return element;
        }
        /// <summary>
        /// Нажимает на элемент если элемент не является null
        /// </summary>
        /// <param name="element"></param>
        /// <param name="delay">задерка после клика. По умолчанию 2 с.</param>
        /// <returns>Возвращает элемент обратно</returns>
        public static AndroidElement ClickIfNotNull(this AndroidElement element, int delay = 2000)
        {
            if (element != null)
            {
                element.Click();
                Thread.Sleep(delay);
            }
            return element;
        }
        /// <summary>
        /// Кликает на элемент с задержкой
        /// </summary>
        /// <param name="element"></param>
        /// <param name="delay">Задержка 2 секунды по умолчанию</param>
        /// <returns>Возвращает элемент обратно</returns>
        public static AndroidElement Click(this AndroidElement element, int delay = 2000)
        {
            element.Click();
            Thread.Sleep(TimeSpan.FromSeconds(delay));
            return element;
        }
    }
    public class Elements
    {
        public static AndroidDriver<AndroidElement> _driver;
        public static string _device_ser;
        public Elements(AndroidDriver<AndroidElement> driver, string device_ser)
        {
            _driver = driver;
            _device_ser = device_ser;
        }

        /// <summary>
        /// Проверяет наличие элемента на странице, применяя различные условия ожидания.
        /// </summary>
        /// <param name="by">Локатор для поиска элемента.</param>
        /// <param name="timeout">Таймаут ожидания в секундах.</param>
        /// <param name="isClickable">Указывает, нужно ли ждать, пока элемент станет кликабельным.</param>
        /// <param name="isVisible">Указывает, нужно ли ждать, пока элемент станет видимым.</param>
        /// <param name="textShouldBePresent">Текст, наличие которого необходимо проверить в элементе.</param>
        /// <returns>Возвращает <see cref="AndroidElement"/> если элемент удовлетворяет условиям; иначе возвращает <c>null</c>.</returns>

        public AndroidElement Check(By by, int timeout = 5, bool isClickable = false, bool isVisible = true, string textShouldBePresent = null)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout));
            try
            {
                AndroidElement el = null;
                if (isClickable)
                    el = wait.Until(ExpectedConditions.ElementToBeClickable(by)) as AndroidElement;
                else if (isVisible)
                    el = wait.Until(ExpectedConditions.ElementIsVisible(by)) as AndroidElement;
                else
                    el = wait.Until(ExpectedConditions.ElementExists(by)) as AndroidElement;
                if (textShouldBePresent != null && el != null && !el.Text.Contains(textShouldBePresent))
                    return null;
                return el;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        /// <summary>
        /// Пытается найти элемент на странице, периодически повторяя попытки в течение указанного времени.
        /// Используется для обработки элементов, которые могут динамически загружаться на странице.
        /// </summary>
        /// <param name="by">Локатор для поиска элемента.</param>
        /// <param name="timeout">Таймаут ожидания в секундах для каждой попытки поиска.</param>
        /// <param name="countCheck">Количество попыток проверки элемента.</param>
        /// <param name="sleepInterval">Интервал в миллисекундах между попытками проверки.</param>
        /// <param name="isClickable">Указывает, нужно ли ждать, пока элемент станет кликабельным.</param>
        /// <param name="isVisible">Указывает, нужно ли ждать, пока элемент станет видимым.</param>
        /// <param name="textShouldBePresent">Текст, наличие которого необходимо проверить в элементе.</param>
        /// <returns>Возвращает <see cref="AndroidElement"/> если элемент удовлетворяет условиям после всех попыток; иначе возвращает <c>null</c>.</returns>

        public AndroidElement LazyLoading(By by, int timeout = 5, int countCheck = 5, int sleepInterval = 1000, bool isClickable = false, bool isVisible = true, string textShouldBePresent = null)
        {
            AndroidElement elem = Check(by, timeout, isClickable: isClickable, isVisible: isVisible, textShouldBePresent: textShouldBePresent);
            if (elem == null)
                return elem;
            for (int i = 0; i < countCheck; i++)
            {
                IList<AndroidElement> elements = _driver.FindElements(by);
                if (elements.Count != 0)
                {
                    if (elem.Id != elements[0].Id)
                        elem = elements[0];
                }
                Thread.Sleep(sleepInterval);
            }
            return elem;
        }
        /// <summary>
        /// Выполняет многократные попытки найти элементы на странице в течение заданного количества попыток и интервалов, обеспечивая обработку элементов, которые могут динамически загружаться или обновляться на странице.
        /// </summary>
        /// <param name="by">Локатор для поиска элементов.</param>
        /// <param name="counts">Количество попыток для поиска элементов.</param>
        /// <param name="milisecondsInterval">Интервал в миллисекундах между попытками поиска.</param>
        /// <param name="countCheck">Количество дополнительных проверок для устойчивости найденных элементов после первоначального обнаружения.</param>
        /// <param name="sleepInterval">Интервал в миллисекундах между дополнительными проверками.</param>
        /// <returns>Список найденных элементов или <c>null</c>, если элементы не найдены после всех попыток.</returns>

        public List<AndroidElement> LazyLoadingElemenets(By by, int counts = 5, int milisecondsInterval = 1000, int countCheck = 5, int sleepInterval = 1000)
        {
            List<AndroidElement> elems = null;
            for (int i = 0; i < counts; i++)
            {
                IList<AndroidElement> elements = _driver.FindElements(by);
                if (elements.Count != 0)
                {
                    elems = elements.ToList();
                    Thread.Sleep(sleepInterval);
                    break;
                }
                Thread.Sleep(milisecondsInterval);
            }
            if (elems == null)
                return elems;
            for (int i = 0; i < countCheck; i++)
            {
                IList<AndroidElement> elements = _driver.FindElements(by);
                if (elems[0].Id != elements[0].Id)
                    elems = elements.ToList();
                Thread.Sleep(sleepInterval);
            }
            return elems;
        }
        //public AndroidElement CheckElementId(string id, int timeout, bool isClickable = false)
        //{
        //    AndroidElement elem = Check(id, timeout: timeout, isClickable: click);
        //    return elem;
        //}
        /// <summary>
        /// Отправляет нажатие клавиши "Назад" на устройстве и затем ожидает указанное количество времени.
        /// </summary>
        /// <param name="sleepAfter">Время задержки в миллисекундах после нажатия клавиши, по умолчанию 1000 миллисекунд (1 секунда).</param>

        public void PressKeyBack(int sleepAfter = 1000)
        {
            _driver.PressKeyCode(AndroidKeyCode.Back);
            Thread.Sleep(sleepAfter);
        }
        /// <summary>
        /// Выполняет свайп на экране устройства от одного элемента к другому.
        /// </summary>
        /// <param name="driver">Экземпляр драйвера, используемый для взаимодействия с устройством.</param>
        /// <param name="firstElement">Элемент, от которого начинается свайп.</param>
        /// <param name="secondElement">Элемент, к которому направлен свайп.</param>
        /// <param name="delay">Задержка менжу первым и вторым элементом</param>

        public void Swipe(AndroidDriver<AndroidElement> driver, AndroidElement firstElement,  AndroidElement secondElement,int delay = 500)
        {
            TouchAction touchAction = new TouchAction(driver);
            touchAction
                .Press(firstElement)
                .Wait(delay)
                .MoveTo(secondElement)
                .Release()
                .Perform();
        }
    }
}

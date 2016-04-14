using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        static IWebDriver driverGC;
        static ChromeOptions options = new ChromeOptions();
        System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> curInputElements, curAElements, curImgElements, curSpanElements, curRoleElements;
        static UInt16 b12Profile = 1;
        
        
        //System.Threading.Thread myThread = new System.Threading.Thread(myTest);
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            driverGC.Quit();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            myTestStart();
        }
        public void myTestStart()
        {
            
            driverGC = new ChromeDriver(@"C:\", options, new TimeSpan(0, 0, 0, 5));
            driverGC.Navigate().GoToUrl("http://10.17.0.48/b12data");
            driverGC.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(5));

            leechElements();

            
            foreach (IWebElement elem in curInputElements)
            {
                string elemName;               
                elemName = elem.GetAttribute("id");
                richTextBox1.AppendText(elemName + "\n");
            }
            if(richTextBox1.Text == "")
            {
                updateStatus("Page Loading Error");
            }
            else if(richTextBox1.Find("kullanici") != -1)
            {
                updateStatus("Page Has Been Successfully Loaded");
                updateStatus("Trying To Login");
                myStep1();
            }
        }

        public void myStep1()
        {
            foreach (IWebElement elem in curInputElements)
            {
                string elemName;

                elemName = elem.GetAttribute("ID");
                if(elemName == "kullanici")
                {
                    elem.SendKeys("MASTER");
                }
                else if(elemName == "Sifre")
                {
                    elem.SendKeys("12345A");
                    elem.Submit();
                    break;
                }
                
            }

            //System.Threading.Thread.Sleep(500);
            //bool loadOkFlag = false;
            //while (loadOkFlag == false)
            //{
            //    System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> tempElements;
            //    tempElements = driverGC.FindElements(By.TagName("div"));
            //    foreach (IWebElement elem in tempElements)
            //    {
            //        string elemName;
            //        elemName = elem.GetAttribute("id");
            //        if (elemName == "logo")
            //        {
            //            loadOkFlag = true;
            //            break;
            //        }
            //    }
            //}



            leechElements();
            richTextBox1.Text = "";
            IWebElement baglanButton = null;
            foreach (IWebElement elem in curInputElements)
            {
                string elemName;
                elemName = elem.GetAttribute("id");
                if(elemName == null || elemName == "")
                {
                    elemName = "hidden, value = " + elem.GetAttribute("value");
                }
                if(elemName == "baglan")
                {
                    baglanButton = elem;
                }
                richTextBox1.AppendText(elemName + "\n");
            }
            if (richTextBox1.Text == "")
            {
                updateStatus("Page Loading Error");
            }
            else if (richTextBox1.Find("kullanici") != -1)
            {
                updateStatus("Username or Password Doesn't Match! Stopped.");
            }
            else if(richTextBox1.Find("searchListView") != -1)
            {
                updateStatus("Login Successful");
                if(richTextBox1.Find("value = 1") != -1 && richTextBox1.Find("value = 21") != -1 && richTextBox1.Find("value = 22") != -1)
                {
                    updateStatus("Profile List Is Up-To-Date");
                    curRoleElements = driverGC.FindElements(By.CssSelector("[role=\"option\"]"));
                    foreach (IWebElement elem in curRoleElements)
                    {
                        IWebElement subElement = null;
                        subElement = elem.FindElement(By.TagName("input"));

                        string elemName;
                        elemName = subElement.GetAttribute("value");
                        if (elemName == b12Profile.ToString())
                        {
                            elem.Click();
                            baglanButton.Click();
                            break;
                        }
                    }

                }
                else
                {
                    updateStatus("Profile List Is Out-Dated or Missing");
                }
            }

        }

        public void updateStatus(string statusString)
        {
            richTextBox3.AppendText(statusString + "\n");
        }

        public void leechElements()
        {
            curInputElements = driverGC.FindElements(By.TagName("input"));
            curAElements = driverGC.FindElements(By.TagName("a"));
            curImgElements = driverGC.FindElements(By.TagName("img"));
            curSpanElements = driverGC.FindElements(By.TagName("span"));
        }
    }
}

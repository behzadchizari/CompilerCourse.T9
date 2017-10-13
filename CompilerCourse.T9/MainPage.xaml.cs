using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CompilerCourse.T9
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        #region Variables

        Dictionary<string, string> Words = new Dictionary<string, string>();


        #endregion

        #region Ctor


        public MainPage()
        {
            this.InitializeComponent();
            this.WordInit();
        }

        #endregion

        #region Methods

        //Normalize : Remove Special Charachters
        public string RemoveDiacritics(string clearText)
        {
            string normalizedText = clearText.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();
            foreach (char ch in normalizedText)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        //Encode String to T9 Number    
        private string EncodeString(string word)
        {
            string result = "";
            try
            {
                result = this.RemoveDiacritics(word).ToLower();
                // Remove digits.
                result = Regex.Replace(result, "[2-9]", string.Empty);
                // Translate to T9
                result = Regex.Replace(result, "[abc]", "2");
                result = Regex.Replace(result, "[def]", "3");
                result = Regex.Replace(result, "[ghi]", "4");
                result = Regex.Replace(result, "[jkl]", "5");
                result = Regex.Replace(result, "[mno]", "6");
                result = Regex.Replace(result, "[pqrs]", "7");
                result = Regex.Replace(result, "[tuv]", "8");
                result = Regex.Replace(result, "[wxyz]", "9");
                result = Regex.Replace(result, "[^2-9]", " ");
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        //Initial Word Dictionary
        private void WordInit()
        {
            Words.Add("apple", EncodeString("apple"));
            Words.Add("orang", EncodeString("orang"));
            Words.Add("salam", EncodeString("salam"));
            Words.Add("baba", EncodeString("baba"));
            Words.Add("maman", EncodeString("maman"));
            Words.Add("bro", EncodeString("bro"));
            Words.Add("Hi", EncodeString("Hi"));
        }


        #endregion


        #region Events

        private void SearchAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var suggestion = Words.Where(x => x.Value.StartsWith(sender.Text) || x.Value.StartsWith(sender.Text.ToLower()) || x.Value.StartsWith(sender.Text.ToUpper())).Select(x => x.Key).ToList();
            SearchAutoSuggestBox.ItemsSource = suggestion;
        }

        private void SearchAutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
        }

        private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var word = Words.Keys.FirstOrDefault(x => x == SearchAutoSuggestBox.Text);

                if (word == null)
                    Words.Add(SearchAutoSuggestBox.Text, EncodeString(SearchAutoSuggestBox.Text));
                SearchAutoSuggestBox.Text = "";
            }
            catch (Exception)
            {
            }
        }






        private void BtnNum2_OnClick(object sender, RoutedEventArgs e) { }



        #endregion

        private void BtnNum_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            SearchAutoSuggestBox.Text += btn.Tag.ToString();
        }
    }
}

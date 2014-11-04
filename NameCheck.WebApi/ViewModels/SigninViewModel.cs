using System;

namespace NameCheck.WebApi
{
    public class SigninViewModel
    {
        public string ReturnUrl { get; set; }
        public string Error { get; set; }
        public bool HasError { get { return !String.IsNullOrWhiteSpace(Error); } }
    }
}
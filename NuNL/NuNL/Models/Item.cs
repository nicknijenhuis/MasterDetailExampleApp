using System;

namespace NuNL.Models
{
    public class Item : BaseDataObject
    {
        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private Uri _uri = null;
        public Uri Link
        {
            get { return _uri; }
            set { SetProperty(ref _uri, value); }
        }

        private DateTime _pubDate;
        public DateTime PubDate
        {
            get { return _pubDate; }
            set { SetProperty(ref _pubDate, value); }
        }

        private Uri _encloseUrl = null;
        public Uri EncloseUrl
        {
            get { return _encloseUrl; }
            set { SetProperty(ref _encloseUrl, value); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;
using System.Web.Mvc;

namespace Cronus.ViewModels
{
    public class FavoriteViewModel
    {
        public IEnumerable<activity> Activities { get; set; }
        public IEnumerable<favorite> Favorites { get; set; }
        public SelectList ActivityNames { get; set; }
        public SelectList UserFavorites { get; set; }
        public int selectedActivityID { get; set; }
    }
}
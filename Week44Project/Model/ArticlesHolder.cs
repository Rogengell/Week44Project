using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week44Project.Model
{
    internal class ArticlesHolder
    {
        public string articlesNumber { get; }

        public string articles { get; }

        public ArticlesHolder(string articlesNumber, string articles) 
        { 
            this.articlesNumber = articlesNumber;
            this.articles = articles;
        }
    }
}

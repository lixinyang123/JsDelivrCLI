using System;
using System.Collections.Generic;

namespace JSDelivrCLI.Models
{
    public class Links
    {
        public string npm { get; set; }
        public string homepage { get; set; }
        public string repository { get; set; }
        public string bugs { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public string url { get; set; }
        public string email { get; set; }
        public string username { get; set; }
    }

    public class Publisher
    {
        public string username { get; set; }
        public string email { get; set; }
    }

    public class Maintainer
    {
        public string username { get; set; }
        public string email { get; set; }
    }

    public class Package
    {
        public string name { get; set; }
        public string scope { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public List<string> keywords { get; set; }
        public DateTime date { get; set; }
        public Links links { get; set; }
        public Author author { get; set; }
        public Publisher publisher { get; set; }
        public List<Maintainer> maintainers { get; set; }
    }

    public class Detail
    {
        public double quality { get; set; }
        public double popularity { get; set; }
        public double maintenance { get; set; }
    }

    public class Score
    {
        public double final { get; set; }
        public Detail detail { get; set; }
    }

    public class Flags
    {
        public bool unstable { get; set; }
    }

    public class Object
    {
        public Package package { get; set; }
        public Score score { get; set; }
        public double searchScore { get; set; }
        public Flags flags { get; set; }
    }

    public class SearchInfo
    {
        public List<Object> objects { get; set; }
        public int total { get; set; }
        public string time { get; set; }
    }
}
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace DBInterface
{
    public static class Query
    {
        public static string ConcatStr(string first, string del, string last)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("concat({0},{1},{2})", first, del, last);
           
#endif
#if MsSql
                QuStr = string.Format("{0}+{1}+{2}", first, del, last);
#endif
            return QuStr;
        }
        public static string Cast(string FieldName, int length)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("cast({0} as char({1}))", FieldName, length);
#endif
#if MsSql
                QuStr = string.Format("cast({0} as nvarchar({1}))", FieldName,length);;
#endif

            return QuStr;
        }
        public static string QryCurDate()
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = "Now()";
#endif
#if MsSql
        QuStr = "getdate()";
#endif
            return QuStr;
        }
        public static string QryFieldToDate(string FieldName)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("str_to_date({0},'%d/%m/%Y')", FieldName);
#endif
#if MsSql
        QuStr = string.Format("convert(datetime,{0} ,103)", FieldName);
#endif
            return QuStr;
        }
        public static string QrydateTostrRd(string FieldName, int fmt)
        {
            string QuStr = string.Empty;


#if MySql

            switch (fmt)
            {
                case 103:
                    QuStr = string.Format("DATE_FORMAT({0},'%d/%m/%Y')", FieldName);
                    break;
                case 106:
                    QuStr = string.Format("DATE_FORMAT({0},'%d %b %Y')", FieldName);
                    break;
                default:
                    QuStr = string.Format("DATE_FORMAT({0},'%d/%m/%Y')", FieldName);
                    break;
            }
#endif
#if MsSql
        QuStr = string.Format("convert(varchar(32),{0},{1})", FieldName, fmt);
#endif
            return QuStr;
        }
        public static string QrydateAsStr(string datestr)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("str_to_date('{0}','%d/%m/%Y')", datestr);
#endif
#if MsSql
        QuStr = string.Format("convert(datetime,'{0}' ,103)", datestr);
#endif
            return QuStr;
        }
        public static string GetDateWithTime(string datestr)
        {
            string sqlStr = string.Empty;
#if MsSql
        sqlStr = string.Format("convert(VARCHAR(6),{0},6)+' '+CONVERT(VARCHAR(5),{0}, 114)", datestr);
#endif
#if MySql
            sqlStr = string.Format("DATE_FORMAT({0},'%d %b %H:%i')", datestr);
#endif
            return sqlStr;
        }

        public static string QryStrToFullDateTime(string datestr)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("str_to_date({0},'%m/%d/%Y %r')", datestr);
#endif
#if MsSql
        QuStr = string.Format("convert(datetime,'{0}' ,100)", datestr);
#endif
            return QuStr;
        }

        public static string QrydateAsDateTime(DateTime FieldName)
        {
            return QrydateAsStr(FieldName.ToString("dd/MM/yyyy"));
        }
        public static string GetTop(string count)
        {

            string QuStr = string.Empty;

#if MsSql
        QuStr = string.Format("top({0}) ", count);
#endif
            return QuStr;
        }
        public static string GetLimit(string count)
        {

            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("limit {0}", count);
#endif
            return QuStr;
        }
        public static string QryGetLastInsID()
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = "select last_insert_id()";
#endif
#if MsSql
        QuStr = "select @@IDENTITY";
#endif
            return QuStr;
        }
        public static string QrySubString(string FieldName, int len)
        {
            string QuStr = string.Empty;
#if MySql

            QuStr = string.Format("CONCAT(LEFT({0},{1}),' ...')", FieldName, len);
#endif
#if MsSql
        QuStr = string.Format("subString({0},0,{1})", FieldName, len);
#endif
            return QuStr;
        }
        public static string QryAddString(string str1, string str2, string del)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("CONCAT_WS({0},{1},{2})", del, str1, str2);
#endif
#if MsSql
        QuStr = string.Format("{0}{1}{2}", str1, del, str2);
#endif
            return QuStr;
        }
        public static string QryFieldToStrToDate(string FieldName, int fmt)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("str_to_date(DATE_FORMAT({0},'%d/%m/%Y'),'%d/%m/%Y')", FieldName, fmt);
#endif
#if MsSql
        //QuStr = string.Format("convert(datetime,convert(varchar(32),{0},{1}) ,103)", FieldName, fmt);
        QuStr = string.Format("convert(datetime,{0},{1})", FieldName, fmt);
#endif
            return QuStr;
        }
        public static string QryFieldToDateToStr(string FieldName, int fmt)
        {
            string QuStr = string.Empty;
#if MySql
                        QuStr = string.Format("DATE_FORMAT(str_to_date({0},'%d/%m/%Y'),'%d/%m/%Y')", FieldName, fmt);
#endif
#if MsSql
                      QuStr = string.Format("convert(varchar(32),convert(datetime,{0},103),{1})", FieldName, fmt);
#endif
            return QuStr;
        }
        public static string Qryisnull(string FieldName, string Default)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("ifnull({0},{1})", FieldName, Default);
#endif
#if MsSql
        QuStr = string.Format("isnull({0},{1})", FieldName, Default);
#endif
            return QuStr;
        }
        public static string QryisnullStr(string FieldName, string Default)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = string.Format("ifnull({0},'{1}')", FieldName, Default);
#endif
#if MsSql
        QuStr = string.Format("isnull({0},'{1}')", FieldName, Default);
#endif
            return QuStr;
        }
        public static string QryGetPageKey(string PageTitle, string PublicationDate, string EditionCode)
        {
            string QuStr = string.Empty;
#if MySql
            QuStr = QryAddString(QryAddString(PageTitle, QryFieldToDateToStr(PublicationDate, 103), "'$'"), EditionCode, "'$'");
#endif
#if MsSql
        QuStr = QryAddString(QryAddString(PageTitle, QryFieldToDateToStr(PublicationDate, 103), "+'$'+"), EditionCode, "+'$'+");
#endif
            return QuStr;
        }
        public static string AnyWordsSearch(string field, string sendWords)
        {
            //   sendWords = sendWords.Replace("'", "");
            string[] word = sendWords.Split(" ".ToCharArray());
            string text = " ";
            int i = 0;

#if MySql
            //foreach (string strWord in word)
            //{
            //    if (i == 0)
            //    {

            //    }
            //    else
            //    {
            //        text = text + "and MATCH (" + field + ") AGAINST ('" + QuotedString(strWord.Trim()).Trim() + "' IN BOOLEAN MODE)";
            //    }
            //    i++;
            // }
            text = text + "MATCH (" + field + ") AGAINST ('" + QuotedString(sendWords.Trim()).Trim() + "' IN BOOLEAN MODE)";
#endif
#if MsSql


        foreach (string strWord in word)
        {
            if (i == 0)
            {
                text = text + "Contains (StoryContent, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"') or Contains (Story_title, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"') ";
            }
            else
            {
              
                text = text + "or   Contains (StoryContent, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"') or Contains (Story_title, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"') ";
            }
            i++;

        }
#endif
            return text;
        }
        public static string AllWordsSearch(string field, string sendWords)
        {
            //sendWords = sendWords.Replace("'", "");
            string[] word = sendWords.Split(" ".ToCharArray());
            sendWords = "+" + sendWords.Replace(" ", " +");
            string text = "(";
            int i = 0;

#if MySql
                            text = text + "MATCH (" + field + ") AGAINST ('" + QuotedString(sendWords.Trim()).Trim() + "' IN BOOLEAN MODE)";
#endif
#if MsSql
                        foreach (string strWord in word)
                        {
                            if (i == 0)
                            {
                               
                                text = text + "(Contains (StoryContent, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"') or Contains (Story_title, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"')) ";

                            }
                            else
                            {
                               
                                text = text + "and   (Contains (StoryContent, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"') or Contains (Story_title, N'\"" + QuotedString(strWord.Trim()).Trim() + "\"')) ";
                            }
                            i++;

                        }
#endif
            return text + ")";

        }

        public static string KeyWordsSearch(string field, string sendWords)
        {

            string[] word = sendWords.Split(" ".ToCharArray());
            sendWords = "+" + sendWords.Replace(" ", " +");
            string text = "(";
#if MySql
                    text = text + " " + field + "like '%" + sendWords.Trim() + "%'";
#endif
#if MsSql
            int i = 0;
                                    foreach (string strWord in word)
                                    {
                                        if (i == 0)
                                        {

                                            text = text + " Keywords like N'%" + QuotedString(strWord.Trim()).Trim() + "%'";

                                        }
                                        else
                                        {
                                            text = text + " Keywords like N'%" + QuotedString(strWord.Trim()).Trim() + "%'";
                                           
                                        }
                                        i++;

                                    }
#endif
            return text + ")";

        }

        public static string ExactWordsSearch(string field, string sendWords, int type)
        {
            string text = string.Empty;
            sendWords = sendWords.Replace("\"", "");
#if MySql
            if (type == 1)
                text = text + "news.StoryContent like '% " + sendWords + " %' or news.story_title like '%" + sendWords + "%'";
            else if (type == 2)
                text = text + "picture.picture_caption like '% " + sendWords + " %' or picture.picture_title like '%" + sendWords + "%'";
            //text = text + "MATCH (" + field + ") AGAINST ('" + QuotedString( sendWords.Trim()).Trim() + "' IN BOOLEAN MODE)";            
#endif
#if MsSql
        string[] FiledArry = field.Split(",".ToCharArray());
        foreach (string f in FiledArry)
            text = text + "Contains (" + f + ", N'" + "\"" + QuotedString(sendWords.Trim()).Trim() + "\"" + "') or ";

        text = text.Substring(0, text.Length - 3);
#endif
            return text;
        }
        public static string QuotedString(string source)
        {
            Regex objreg = new Regex("'");
            return objreg.Replace(source, "''");
        }
        public static string GetActionTime()
        {
            string sqlStr = string.Empty;
#if MsSql
        sqlStr = string.Format("convert(VARCHAR(6),ActionTime,6)+' '+CONVERT(VARCHAR(5),ActionTime, 114)");
#endif
#if MySql
            sqlStr = string.Format("DATE_FORMAT(ActionTime,'%d %b %H:%i')");
#endif
            return sqlStr;
        }

        public static string GetConcatnatedCategories(string field)
        {
            string QuStr = string.Empty;
#if MySql
                 QuStr = string.Format("ifnull(GetConcatnatedCategories({0}),'U')", field);
#endif
#if MsSql
                  QuStr = string.Format("isnull(DBO.GetConcatnatedCategories({0}),'U')", field);
#endif
            return QuStr;
        }
        public static string GetPublicationTime()
        {
            string sqlStr = string.Empty;
#if MsSql
        sqlStr = string.Format("convert(VARCHAR(6),convert(datetime,publication_date,103),6)");
#endif
#if MySql
            sqlStr = string.Format("date_format(str_to_date(publication_date, '%d/%m/%y'),'%d %b')");
#endif
            return sqlStr;
        }
        public static string GetPictureSize()
        {
            string sqlStr = string.Empty;
#if MsSql
        sqlStr = string.Format("ROUND(CAST(picture.size as float)/cast((1024*1024) as float),2)");
#endif
#if MySql
            sqlStr = string.Format("ROUND((picture.size)/1024)");
#endif
            return sqlStr;
        }
    }
}
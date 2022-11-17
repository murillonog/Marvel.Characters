namespace Marvel.Characters.Application.Filters
{
    public class CharacterRequestFilter
    {

        public int? Page { get; set; } = 0;
        public int? Size { get; set; } = 10;
        public string OrderBy { get; set; } = "Name";
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? ModifiedSince { get; set; }
        public bool? IsFavorite { get; set; }

        public string GetClauseWhere()
        {
            string clause = string.Empty;

            clause += GetNameClause(clause);
            clause += GetDescriptionClause(clause);
            clause += GetModifiedSinceClause(clause);
            clause += GetIsFavoriteClause(clause);
            clause += OrderClause();

            return clause;
        }

        private string GetIsFavoriteClause(string clause)
        {
            if (IsFavorite != null)
            {
                string value = IsFavorite.Value ? "1" : "0";
                if (clause.Contains("WHERE"))
                    return $" AND Favorite = {value}";

                else
                    return $"WHERE  Favorite = {value}";
            }
            return string.Empty;
        }

        private string GetModifiedSinceClause(string clause)
        {
            if (ModifiedSince != null)
            {
                if (clause.Contains("WHERE"))
                    return $" AND Modified > {ModifiedSince.Value.ToString("yyyy-MM-dd HH:mm:ss")}";

                else
                    return $"WHERE Modified > {ModifiedSince.Value.ToString("yyyy-MM-dd HH:mm:ss")}";
            }
            return string.Empty;
        }

        private string GetDescriptionClause(string clause)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                if (clause.Contains("WHERE"))
                    return $" AND Description LIKE '%{Description}%'";

                else
                    return $"WHERE  Description LIKE '%{Description}%'";
            }
            return string.Empty;
        }

        private string GetNameClause(string clause)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                if (clause.Contains("WHERE"))
                    return $" AND Name LIKE '%{Name}%'";

                else
                    return $"WHERE Name LIKE '%{Name}%'";
            }
            return string.Empty;
        }

        private string OrderClause()
        {
            return $" ORDER BY {OrderBy}";
        }
    }
}

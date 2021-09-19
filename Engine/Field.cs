namespace Engine
{
    public class Field
    {
        public bool? HasMine { get; }

        public int AdjacentMines { get; set; }

        public bool IsOpen { get; set; }

        public bool IsMarked { get; set; }

        public Field(bool hasMine) => HasMine = hasMine;

        public Field(Field field)
        {
            IsOpen = field.IsOpen;
            IsMarked = field.IsMarked;
            if (!IsMarked) AdjacentMines = field.AdjacentMines;
        }
    }
}

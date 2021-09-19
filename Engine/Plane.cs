namespace Engine
{
    public class Plane
    {
        private (bool hasMine, int adjacentMines, bool isOpen, bool isMarked)[,] fieldTest;

        #region Private Fields

        private static int _numberOfFields;

        private readonly Field[,] _fields;

        private int? _rowCount;
        private int? _columnCount;

        #endregion Private Fields

        #region Public Properties

        public int RowCount => _rowCount ??= _fields.GetLength(0);

        public int ColumnCount => _columnCount ??= _fields.GetLength(1);

        public bool Finished { get; private set; }

        public bool IsSuccess { get; private set; }

        public int NumberOfMarkedFields { get; private set; }

        public int NumberOfOpenFields { get; private set; }

        public int NumberOfMines { get; private set; }

        #endregion Public Properties

        #region Constructor

        public Plane(Field[,] fields)
        {
            _fields = fields;

            for (int row = 0; row < RowCount; row++)
                for (int column = 0; column < ColumnCount; column++)
                {
                    _fields[row, column].AdjacentMines = GetNumberOfAdjacentMines(row, column);
                    if (_fields[row, column].HasMine == true) NumberOfMines++;
                }

            _numberOfFields = RowCount * ColumnCount;
        }

        #endregion Constructor

        #region Public Methods

        public Field[,] GetFields()
        {
            Field[,] fields = new Field[_fields.GetLength(0), _fields.GetLength(1)];
            for (int row = 0; row < RowCount; row++)
                for (int column = 0; column < ColumnCount; column++)
                    if (_fields[row, column].IsOpen || _fields[row, column].IsMarked)
                        fields[row, column] = new Field(_fields[row, column]);
            return fields;
        }

        public void Open(int row, int column)
        {
            if (_fields[row, column].IsOpen || _fields[row, column].IsMarked) return;

            _fields[row, column].IsOpen = true;

            NumberOfOpenFields++;

            if (_fields[row, column].HasMine == true)
            {
                Finished = true;
                return;
            }

            if (_fields[row, column].AdjacentMines == 0)
            {
                for (int row0 = row - 1; row0 <= row + 1; row0++)
                    for (int column0 = column - 1; column0 <= column + 1; column0++)
                        if (row0 >= 0 && column0 >= 0 &&
                            row0 < RowCount && column0 < ColumnCount &&
                            !_fields[row0, column0].IsOpen)
                            Open(row0, column0);
            }

            if (NumberOfMarkedFields == NumberOfMines &&
                NumberOfOpenFields == _numberOfFields - NumberOfMines)
            {
                Finished = true;
                if (!_fields[row, column].HasMine == true) IsSuccess = true;
            }
        }

        public void Mark(int row, int column)
        {
            if (_fields[row, column].IsMarked)
            {
                _fields[row, column].IsMarked = false;
                NumberOfMarkedFields--;
            }
            else if (!_fields[row, column].IsOpen)
            {
                _fields[row, column].IsMarked = true;
                NumberOfMarkedFields++;
            }

            if (NumberOfMarkedFields == NumberOfMines &&
                NumberOfOpenFields == _numberOfFields - NumberOfMines)
            {
                Finished = true;
                IsSuccess = true;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private int GetNumberOfAdjacentMines(int row1, int column1)
        {
            int result = 0;
            for (int row = row1 - 1; row <= row1 + 1; row++)
                for (int column = column1 - 1; column <= column1 + 1; column++)
                    if (row >= 0 && column >= 0 &&
                        row < RowCount && column < ColumnCount &&
                        _fields[row, column].HasMine == true)
                        result++;
            return result;
        }

        #endregion Private Methods
    }
}

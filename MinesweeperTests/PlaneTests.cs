using Engine;
using System;
using Xunit;

namespace MinesweeperTests
{
    public class PlaneTests
    {
        #region MarkField

        [Fact]
        public void MarkField_FieldIsMarked()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Mark(2, 0);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            bool?[,] expected = new bool?[,]
            {
                { null, null, null },
                { null, null, null },
                { true, null, null }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.IsMarked));
        }

        [Fact]
        public void TryOpenMarkedField_FieldNotOpen()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Mark(0, 0);
            plane.Open(0, 0);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            bool?[,] expected = new bool?[,]
            {
                { false, null, null },
                { null,  null, null },
                { null,  null, null }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.IsOpen));
        }


        [Fact]
        public void TryMarkOpenedField_FieldNotMarked()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Open(0, 0);
            plane.Mark(0, 0);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            bool?[,] expected = new bool?[,]
            {
                { false, false, false },
                { false, false, false },
                { null,  false, false }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.IsMarked));
        }

        [Fact]
        public void RemoveMarkFromMarkedField_FieldIsNotMarked()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Mark(0, 0);
            plane.Mark(0, 0);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Null(result[row, column]?.IsMarked));
        }

        #endregion MarkField

        #region MineInCorner

        [Fact]
        public void MineInCorner_OpenNonAdjacentField_OpensAllEmptyFields()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Open(0, 0);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            int?[,] expected = new int?[,]
            {
                {    0, 0, 0 },
                {    1, 1, 0 },
                { null, 1, 0 }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.AdjacentMines));
        }

        [Fact]
        public void MineInCorner_OpenAdjacentField_OpensAdjacentField()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Open(1, 0);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            int?[,] expected = new int?[,]
            {
                { null, null, null },
                {    1, null, null },
                { null, null, null }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.AdjacentMines));
        }

        [Fact]
        public void MineInCorner_OpenMineField_EndsGame()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Open(2, 0);

            // Assert
            Assert.True(plane.Finished);
            Assert.False(plane.IsSuccess);
        }

        [Fact]
        public void MineInCorner_SolvePlane_EndsGame()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Open(0, 0);
            plane.Mark(2, 0);

            // Assert
            Assert.True(plane.Finished);
            Assert.True(plane.IsSuccess);
        }

        #endregion MineInCorner

        #region MineNotInCorner

        [Fact]
        public void MineNotInCorner_OpenNonAdjacentField_OpensConnectedEmptyFields()
        {
            // Arrange
            Plane plane = PlaneWithMineNotInCorner();

            // Act
            plane.Open(1, 3);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            int?[,] expected = new int?[,]
            {
                {    0,    0, 0, 0 },
                {    1,    1, 1, 0 },
                { null, null, 1, 0 },
                {    1,    1, 1, 0 },
                {    0,    0, 0, 0 }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.AdjacentMines));
        }

        [Fact]
        public void MineNotInCorner_OpenAdjacentField_OpensAdjacentField()
        {
            // Arrange
            Plane plane = PlaneWithMineNotInCorner();

            // Act
            plane.Open(1, 2);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            int?[,] expected = new int?[,]
            {
                { null, null, null, null },
                { null, null,    1, null },
                { null, null, null, null },
                { null, null, null, null },
                { null, null, null, null }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.AdjacentMines));
        }

        [Fact]
        public void MineNotInCorner_OpenMineField_EndsGame()
        {
            // Arrange
            Plane plane = PlaneWithMineNotInCorner();

            // Act
            plane.Open(2, 1);

            // Assert
            Assert.True(plane.Finished);
            Assert.False(plane.IsSuccess);
        }

        [Fact]
        public void MineNotInCorner_SolvePlane_EndsGame()
        {
            // Arrange
            Plane plane = PlaneWithMineInCorner();

            // Act
            plane.Open(0, 0);
            plane.Mark(2, 0);
            plane.Open(2, 0);

            // Assert
            Assert.True(plane.Finished);
            Assert.True(plane.IsSuccess);
        }

        #endregion MineNotInCorner

        #region TwoMines

        [Fact]
        public void TwoMines_OpenEmptyField_OpensAllEmptyFields()
        {
            // Arrange
            Plane plane = PlaneWithTwoMines();

            // Act
            plane.Open(1, 3);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            int?[,] expected = new int?[,]
            {
                {    0,    0,    0,    0 },
                {    1,    1,    1,    0 },
                { null, null,    1,    0 },
                { null, null,    2,    1 },
                { null, null, null, null }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.AdjacentMines));
        }

        [Fact]
        public void TwoMines_OpenAdjacentField_OpensAdjacentField()
        {
            // Arrange
            Plane plane = PlaneWithTwoMines();

            // Act
            plane.Open(3, 2);
            Field[,] result = plane.GetFields();

            // Assert
            Assert.False(plane.Finished);
            int?[,] expected = new int?[,]
            {
                { null, null, null, null },
                { null, null, null, null },
                { null, null, null, null },
                { null, null,    2, null },
                { null, null, null, null }
            };
            TestPlane(plane.RowCount, plane.ColumnCount, (row, column) =>
                Assert.Equal(expected[row, column], result[row, column]?.AdjacentMines));
        }

        [Fact]
        public void TwoMines_OpenMineField_EndsGame()
        {
            // Arrange
            Plane plane = PlaneWithTwoMines();

            // Act
            plane.Open(2, 1);

            // Assert
            Assert.True(plane.Finished);
            Assert.False(plane.IsSuccess);
        }

        [Fact]
        public void TwoMines_SolvePlane_EndsGame()
        {
            // Arrange
            Plane plane = PlaneWithTwoMines();

            // Act
            plane.Open(0, 0);
            plane.Open(4, 0);
            plane.Open(2, 0);
            plane.Mark(2, 1);
            plane.Mark(4, 3);

            // Assert
            Assert.True(plane.Finished);
            Assert.True(plane.IsSuccess);
        }

        #endregion TwoMines

        #region Helper Methods

        private static Plane PlaneWithMineInCorner()
        {
            Field[,] fields = new[,]
            {
                { new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false) },
                { new Field(true) , new Field(false), new Field(false) }
            };

            return new Plane(fields);
        }

        private static Plane PlaneWithMineNotInCorner()
        {
            Field[,] fields = new[,]
            {
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(true),  new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(false) }
            };

            return new Plane(fields);
        }

        private static Plane PlaneWithTwoMines()
        {
            Field[,] fields = new[,]
            {
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(true),  new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(true)  }
            };

            return new Plane(fields);
        }

        private static void TestPlane(int rowCount, int columnCount, Action<int, int> action)
        {
            for (int row = 0; row < rowCount; row++)
                for (int column = 0; column < columnCount; column++)
                    action(row, column);
        }

        #endregion Helper Methods
    }
}

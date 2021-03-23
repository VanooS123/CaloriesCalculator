using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CaloriesCalculator
{
     public partial class Form1 : Form
     { 
        private ResultsForm _resultsForm;
        private const int RowsCountPfc = 3;
        private const int CellsCountPfc = 3;
        private CountingCalories _count;
        private int _caloriesNumberWithout;
        private bool _formula;
        private List<int> _changingWeightModes = new List<int>();
        private List<Label> _textBoxesPfc = new List<Label>();
        public Form1()
        {
            InitializeComponent();
        }

        private void ингридиентыBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            IngridientsBindingSource.EndEdit();
            tableAdapterManager.UpdateAll(IngridientsПродуктовDataSet);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            блюдаTableAdapter.Fill(this.калорийностьПродуктовDataSet2.Блюда);
            ингридиентыTableAdapter.Fill(калорийностьПродуктовDataSet1.Ингридиенты);
            ингридиентыTableAdapter.Fill(калорийностьПродуктовDataSet1.Ингридиенты);
            IngridientsTableAdapter.Fill(IngridientsПродуктовDataSet.Ингридиенты);
        }

        private void CountCalories_Click(object sender, EventArgs e)
        {
            _changingWeightModes.Clear();
            _formula = MifflinFormula.Checked;
            if (_resultsForm == null)
            {
                _resultsForm = new ResultsForm
                {
                    Visible = true
                };
            }
            CountCaloriesOnFormula();
            CreateLabelsValuesPfc();
            FillPfcMuscules();
        }
        
        private void CountIdealWeight_Click(object sender, EventArgs e)
        {
            var idealWeightCount = new CountingIdealWeight(this);
            var idealWeight = Math.Round(idealWeightCount.CountIdealWeight(), 1);
            var weightIndex = idealWeightCount.CountWeightIndex();
            IdealWeightDisplay.Text = idealWeight.ToString(CultureInfo.InvariantCulture);
            WeightIndex.Text = weightIndex.ToString(CultureInfo.InvariantCulture);
        }
        
        private void CountCaloriesOnFormula()
        {
            _count = new CountingCalories(this);
            int caloriesNumberMedium;
            int caloriesNumberFast;
            int moreCalories;
            if (_formula)
            {
                _caloriesNumberWithout = _count.CountCaloriesMifflinActivityTypes(out caloriesNumberFast, 
                    out caloriesNumberMedium, out moreCalories);
            }
            else
            {
                _caloriesNumberWithout = _count.CountCaloriesHarrisActivityTypes(out caloriesNumberFast, 
                    out caloriesNumberMedium,out moreCalories);
            }

            AddModesChangingWeightList(_caloriesNumberWithout, caloriesNumberMedium, caloriesNumberFast, 
                moreCalories);
            
            _resultsForm.NotDecreasingWeight.Text = _caloriesNumberWithout.ToString();
            _resultsForm.DecreasingWeight.Text = caloriesNumberMedium.ToString();
            _resultsForm.FastDecreasingWeight.Text = caloriesNumberFast.ToString();
            _resultsForm.MoreWeight.Text = moreCalories.ToString();
        }

        private void AddModesChangingWeightList(params int[] modes)
        {
            foreach (var mode in modes)
            {
                _changingWeightModes.Add(mode);
            }
        }

        private void FillPfcMuscules()
        {
            const float proteinsCoef = 2;
            const float fatsCoef = 0.8f;
            var weight = 0f;
            try
            {
                weight = Convert.ToSingle(Weight.Text);
            }
            catch (Exception)
            {
                Console.WriteLine(@"Введите верные значения в поля параметров");
            }
            
            var coefValue = new float();
            var proteins = 0f;
            var fats = 0f;
            for (int i = 0; i < _textBoxesPfc.Count; i++)
            {
                if (i % 3 == 0)
                {
                    coefValue = proteinsCoef * weight;
                    proteins = coefValue;
                }
                if (i % 3 == 1)
                {
                    coefValue = fatsCoef * weight;
                    fats = coefValue;
                }
                if (i % 3 == 2)
                {
                    coefValue = (Convert.ToInt32(_resultsForm.NotDecreasingWeight.Text) -
                                 (proteins * 4 + fats * 9)) / 4;
                }

                _textBoxesPfc[i].Text = coefValue.ToString(CultureInfo.InvariantCulture);
            }
        }
        
        private void FillPfc()
        {
            var coefParameterPfc = 0d;
            var parameterPfc = 0d;
            var j = 0;
            for (int i = 0; i < _textBoxesPfc.Count; i++)
            {
                if (i % 3 == 0)
                {
                    parameterPfc = Convert.ToDouble(_changingWeightModes[j])  * coefParameterPfc;
                    if (j == 0) 
                    {
                        coefParameterPfc = 0.3 / 4;
                    }
                    if (j == 1)
                    {
                        coefParameterPfc = 0.2 / 9;
                    }
                    if (j == 2)
                    {
                        coefParameterPfc = 0.5 / 4;
                    }
                    
                    j++;
                }
                _textBoxesPfc[i].Text = parameterPfc.ToString(CultureInfo.InvariantCulture);
            }
        }
        
        private void CreateLabelsValuesPfc()
        {
            _textBoxesPfc.Clear();
            _resultsForm.IndicatorValues.Controls.Clear();
            
            var topControl = 0;
            var leftControl = 0;
            
            var tmpTop = (_resultsForm.FatsText.Top + _resultsForm.FatsText.Height / 2) - 
                         (_resultsForm.ProteinText.Top + _resultsForm.ProteinText.Height / 2);
            var tmpLeft = (_resultsForm.WeightDecreaseText.Left + _resultsForm.WeightDecreaseText.Width / 2) - 
                          (_resultsForm.WithoutWeightChangeText.Left + _resultsForm.WithoutWeightChangeText.Width / 2);
            
            /*for (int i = 0; i < RowsCountPfc; i++)
            {*/
                for (int j = 0; j < CellsCountPfc; j++)
                {
                    var labelParam = CreateOneFieldPfc(new Point(leftControl, topControl));
                    _resultsForm.IndicatorValues.Controls.Add(labelParam);
                    _textBoxesPfc.Add(labelParam);
                    topControl += tmpTop;
                }

                /*
                topControl += tmpTop;
                leftControl = 0;
            }*/
        }

        private Label CreateOneFieldPfc(Point controls)
        {
            var labelParam = new Label
            {
                Location = new Point(controls.X,controls.Y),
                Width = _resultsForm.WeightDecreaseText.Width,
                Height = _resultsForm.FatsText.Height,
                Visible = true,
                    
            };
            
            return labelParam;
        } 
        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void IngridientsControl_Enter(object sender, EventArgs e)
        {
            IngridientsBindingNavigator.Visible = true;
        }

        private void IngridientsControl_Leave(object sender, EventArgs e)
        {
            IngridientsBindingNavigator.Visible = false;
        }

        private void DishesControl_Enter(object sender, EventArgs e)
        {
            //Dishe
        }
        
        private void IngridientsDataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var prevCount = NewDishesGrid.Rows.Count;
            var selectedRows = IngridientsDataGridView.SelectedRows;
            NewDishesGrid.Rows.Add(selectedRows);
            
            for (int i = 0; i < selectedRows.Count; i++)
            {
                var selectedCells = selectedRows[i].Cells;
                for (int j = 1; j < selectedCells.Count; j++)
                {
                    NewDishesGrid.Rows[prevCount + i - 1].Cells[j - 1].Value = selectedCells[j].Value;
                }
            }

            CommonColoricity.Text = CountCommonColoricity().ToString(CultureInfo.InvariantCulture);
        }

        private double CountCommonColoricity()
        {
            var rows = NewDishesGrid.Rows;
            var commonColorictity = 0d;
            for (int i = 0; i < rows.Count; i++)
            {
                var ingridientColoricity = Convert.ToSingle(rows[i].Cells[1].Value);
                var ingridientGramms = Convert.ToSingle(rows[i].Cells[2].Value);
                commonColorictity += Math.Round(ingridientColoricity * (ingridientGramms / 100), 0);
            }

            return commonColorictity;
        }

        private void AddingNewDish_Click(object sender, EventArgs e)
        {
            var name = NewDishName.Text;
            var coloricity = Convert.ToInt32(CommonColoricity.Text);
            var gramms = 0;
            var ingridients = "";
            for (int i = 0; i < NewDishesGrid.Rows.Count; i++)
            {
                gramms += Convert.ToInt32(NewDishesGrid.Rows[i].Cells[2].Value);
                ingridients += NewDishesGrid.Rows[i].Cells[0].Value + "  ";
            }
            DishesTable.Rows.Add(name, 
                coloricity, gramms, ingridients);
            AddRowToDB(name, coloricity, gramms, ingridients);

            for (int i = 0; i < IngridientsDataGridView.Rows.Count; i++)
            {
                IngridientsDataGridView.Rows.Remove(IngridientsDataGridView.Rows[i]);
            }
        }

        private void AddRowToDB(string name, int coloricity, int gramms, string ingridients)
        {
            const string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=КалорийностьПродуктов.mdb";
            var connection = new OleDbConnection(connectionString);
            var command = new OleDbCommand
            {
                Connection = connection
            };
            string query =
                "INSERT INTO `Блюда` (НазваниеБлюда, Калорийность, Порция, Состав) VALUES('" +name+"', '"+ coloricity+"', '"+gramms+"', '" +ingridients+ "')";
            command.CommandText = query;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        
        private void IngridientsDataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataGridHeightNormalization(IngridientsDataGridView);
        }

        private void IngridientsDataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataGridHeightNormalization(IngridientsDataGridView);
        }

        private void IngridientsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }
        
        private void IngridientsDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IngridientsRowsIndexesNormalization();
            DataGridHeightNormalization(IngridientsDataGridView);
        }
        
        private void IngridientsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            IngridientsRowsIndexesNormalization();
            DataGridHeightNormalization(IngridientsDataGridView);
        }

        private void IngridientsRowsIndexesNormalization()
        {
            for (int i = 0; i < IngridientsDataGridView.Rows.Count; i++)
            {
                IngridientsDataGridView.Rows[i].Cells[0].Value = i;
            }
        }
        
        private void DataGridHeightNormalization(DataGridView gridToNormalizate)
        {
            gridToNormalizate.Height = gridToNormalizate.Rows.Count * 22 + 32;
        }

     }

    public class CountingCalories
    {
        private const int MaleFactor = 5;
        private const int FeMaleFactor = -161;

        private float _weight;
        private int _height;
        private int _ages;
        private bool _isMale;
        private float _activityIndex;
        private int _genderFactor;
        
        public CountingCalories(Form1 mainForm)
        {
            try
            {
                SetParams(mainForm);
                if ((_weight <= 35 || _weight >= 175) || (_height <= 145 || _height >= 225) ||
                    (_ages <= 7 || _ages >= 100) || (!mainForm.Male.Checked && !mainForm.Female
                        .Checked))
                {
                    throw new Exception("Укажите правильные значения в полях параметров");
                }
            }
            catch (Exception exception)
            {
                _activityIndex = 0;
                MessageBox.Show(exception.Message);
            }
        }

        public int CountCaloriesHarrisActivityTypes(out int fastLoss, out int mediumLoss, out int moreWeight)
        {
            fastLoss = (int) (CountCaloriesHarris() * 0.6);
            mediumLoss = (int) (CountCaloriesHarris() * 0.8);
            moreWeight = (int) (CountCaloriesHarris() * 1.2);
            return CountCaloriesHarris();
        }
        
        public int CountCaloriesMifflinActivityTypes(out int fastLoss, out int mediumLoss, out int moreWeight)
        {
            fastLoss = (int) (CountCaloriesMifflin() * 0.6);
            mediumLoss = (int) (CountCaloriesMifflin() * 0.8);
            moreWeight = (int) (CountCaloriesMifflin() * 1.2);
            return CountCaloriesMifflin();
        }

        private int CountCaloriesHarris()
        {
            SetGenderFactor();
            int caloriesNormal;
            if (_isMale)
            {
                caloriesNormal = (int) ((665.1 + (9.6 * _weight) + (1.85 * _height) - (4.68 * _ages)) * _activityIndex);
            }
            else
            {
                caloriesNormal = (int) ((66.47 + (13.75 * _weight) + (5 * _height) - (6.74 * _ages)) * _activityIndex);
            }
           
            return caloriesNormal;
        }
        
        private int CountCaloriesMifflin()
        {
            SetGenderFactor();
            var caloriesNormal = (int) ((10 * _weight + 6.25 * _height - 5 * _ages + _genderFactor) * 
            _activityIndex);
            return caloriesNormal;
        }
        
        private void SetGenderFactor()
        {
            if (_isMale)
            {
                _genderFactor = MaleFactor;
            }
            else
            {
                _genderFactor = FeMaleFactor;
            }
        }
        
        private void SetParams(Form1 mainForm)
        {
            _weight = Convert.ToSingle(mainForm.Weight.Text);
            _height = Convert.ToInt32(mainForm.Growth.Text);
            _ages = Convert.ToInt32(mainForm.Ages.Text);
            _activityIndex = 1.2f + mainForm.PhysicalActivity.SelectedIndex * 0.175f;
            _isMale = mainForm.Male.Checked;
        }
    }

    public class CountingIdealWeight
    {
        private const float MaleFactor = 2.2f;
        private const float FeMaleFactor = -2.2f;
        
        private float _height;
        private float _currentWeight;
        private float _genderFactor;
        private bool _isMale;
        
        public CountingIdealWeight(Form1 mainForm)
        {
            _currentWeight = Convert.ToSingle(mainForm.IdealWeight.Text);
            _height = Convert.ToSingle(mainForm.IdealHeight.Text);
            _isMale = mainForm.Male.Checked;

            try
            {
                if ((_height <= 145 || _height >= 225) || (!mainForm.Male.Checked && !mainForm.Female
                    .Checked))
                {
                    _height = 0;
                    _genderFactor = 0;
                    throw new Exception("Укажите правильные значения в полях параметров");
                }
            }
            catch (ArgumentException)
            {
                _height = 0;
                _genderFactor = 0;
                MessageBox.Show(@"Укажите значения в полях параметров для расчета веса");
            }
            catch (NullReferenceException)
            {
                _height = 0;
                _genderFactor = 0;
                MessageBox.Show(@"Укажите значения в полях параметров для расчета веса");
            }
            catch (InvalidCastException)
            {
                _height = 0;
                _genderFactor = 0;
                MessageBox.Show(@"Укажите верные данные в полях параметров для расчета веса");
            }
            catch (Exception exception)
            {
                _height = 0;
                _genderFactor = 0;
                MessageBox.Show(exception.Message);
            }
        }

        public double CountIdealWeight()
        {
            SetGenderFactor();
            var idealWeight = Math.Pow(_height / 100, 2) * 21.745 + _genderFactor;
            return idealWeight;
        }

        public float CountWeightIndex()
        {
            var index = (float) Math.Round((_currentWeight / Math.Pow(_height / 100, 2)), 2);
            return index;
        }
        
        private void SetGenderFactor()
        {
            if (_isMale)
            {
                _genderFactor = MaleFactor;
            }
            else
            {
                _genderFactor = FeMaleFactor;
            }
        }
    }
}

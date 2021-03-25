using System.ComponentModel;

namespace CaloriesCalculator
{
    partial class ResultsIngridients
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DishStructure = new System.Windows.Forms.DataGridView();
            this.IngridientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CaloricityDish = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Portion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize) (this.DishStructure)).BeginInit();
            this.SuspendLayout();
            // 
            // DishStructure
            // 
            this.DishStructure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DishStructure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {this.IngridientName, this.CaloricityDish, this.Portion});
            this.DishStructure.Location = new System.Drawing.Point(127, 14);
            this.DishStructure.Margin = new System.Windows.Forms.Padding(5);
            this.DishStructure.Name = "DishStructure";
            this.DishStructure.Size = new System.Drawing.Size(603, 427);
            this.DishStructure.TabIndex = 14;
            // 
            // IngridientName
            // 
            this.IngridientName.HeaderText = "Название ингридиента";
            this.IngridientName.Name = "IngridientName";
            this.IngridientName.Width = 225;
            // 
            // CaloricityDish
            // 
            this.CaloricityDish.HeaderText = "Калорийность блюда";
            this.CaloricityDish.Name = "CaloricityDish";
            this.CaloricityDish.Width = 225;
            // 
            // Portion
            // 
            this.Portion.HeaderText = "Порция";
            this.Portion.Name = "Portion";
            // 
            // ResultsIngridients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 598);
            this.Controls.Add(this.DishStructure);
            this.Font = new System.Drawing.Font("Berlin Sans FB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ResultsIngridients";
            this.Text = "Ингридиенты продукта";
            ((System.ComponentModel.ISupportInitialize) (this.DishStructure)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridViewTextBoxColumn CaloricityDish;
        private System.Windows.Forms.DataGridViewTextBoxColumn IngridientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Portion;

        public System.Windows.Forms.DataGridView DishStructure;

        #endregion
    }
}
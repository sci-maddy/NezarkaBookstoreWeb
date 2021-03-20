using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpingPlatformGame {
	public partial class MainForm : Form {
		private const int LabelWidth = 30; //size of boxes
		private const int LabelHeight = 30;

		private List<Entity> entities = new List<Entity>(); //list of entities
		private List<Label> entityLabels = new List<Label>(); //list of boxes of entities (colorful squares)
		private Random random = new Random();

		public MainForm() {
			InitializeComponent();
		}

		private void RegisterEntity(Entity entity) { //creates entitiy
			entities.Add(entity); 

			entity.Location = new WorldPoint { //randomly spawns the entity in the grid, Location stores center of the entity
				X = random.Next(LabelWidth / 2, worldPanel.Width - LabelWidth / 2).Meters(), //so 30 / 2 is 15 pixels from the walls it has to spawn
				Y = (LabelWidth / 2).Meters() //on the ground
			};

			var label = new Label(); //creating a square for the entity
			label.AutoSize = false;
			label.Width = LabelWidth;
			label.Height = LabelHeight;
			label.BackColor = entity.Color;
			label.SetLocation(entity, worldPanel.Height); //puts it in the grid according to entites Location and size of grid
			entityLabels.Add(label); //adds squares where it is due
			worldPanel.Controls.Add(label);

			if (entity is MovableEntity movableEntity) {  //this also sets bounds for MoveableJumpingEntity!
				movableEntity.Horizontal.LowerBound = (LabelWidth / 2).Meters(); //walls
				movableEntity.Horizontal.UpperBound = (worldPanel.Width - LabelWidth / 2).Meters();
				movableEntity.Horizontal.Speed = 160.MeterPerSeconds(); //horizontal speed - going right
			}

			if (entity is MovableJumpingEntity jumpingEntity) {
				jumpingEntity.Vertical.LowerBound = (LabelHeight / 2).Meters(); //walls
				jumpingEntity.Vertical.UpperBound = (worldPanel.Height - LabelHeight / 2).Meters();
				jumpingEntity.Vertical.Speed = (-200).MeterPerSeconds(); //going down

				var jumpButton = new Button(); //creates extra button for jumping
				jumpButton.Text = entity.ToString();
				jumpButton.Tag = entity;
				jumpButton.Click += JumpButton_Click;
				jumpingPanel.Controls.Add(jumpButton);
			}
		}

		private void JumpButton_Click(object sender, EventArgs e) {
			var entity = (sender as Button)?.Tag as MovableJumpingEntity 
				?? throw new ArgumentException($"Must be a Button with Tag set to a {nameof(MovableJumpingEntity)}", nameof(sender));
			
			entity.Vertical.Speed = Math.Abs(entity.Vertical.Speed.Value).MeterPerSeconds();//reverts gravity - square moves up
		}

		DateTime lastTick = DateTime.Now;

		private void updateTimer_Tick(object sender, EventArgs e) {
			var now = DateTime.Now;
			var deltaSeconds = ((now - lastTick).TotalSeconds).Seconds();
			lastTick = now;

			for (int i = 0; i < entities.Count; i++) {
				entities[i].Update(deltaSeconds); //passes how long time has passed as an argument
				entityLabels[i].SetLocation(entities[i], worldPanel.Height); //draws the square on the new place
			}
		}

		private void joeButton_Click(object sender, EventArgs e) => RegisterEntity(new Joe());

		private void janeButton_Click(object sender, EventArgs e) => RegisterEntity(new Jane());

		private void jackButton_Click(object sender, EventArgs e) => RegisterEntity(new Jack());

		private void jillButton_Click(object sender, EventArgs e) => RegisterEntity(new Jill());
	}

	static class ControlExtensions {
		public static void SetLocation(this Control control, Entity entity, int worldHeight) { //EXTENSION METHOD!
			control.Left = (int) entity.Location.X.Value - control.Width / 2; // draws entity on its location, pixels are integers
			control.Top = worldHeight - (int) entity.Location.Y.Value - control.Height / 2;
		}
	}

}

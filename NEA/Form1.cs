using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace NEA
{
    public partial class NEA_Proj : Form
    {
        List<Position> positions = new List<Position>();
        int[] CurrentGridPosition = { 0, 0 };

        private int CurrentRotation = 0;
        private int CurrentShip = 0;

        private int SpaceStationCounter = 0;
        private int InhabitableCounter = 0;
        private int AsteroidCounter = 0;

        private bool UsingArrows = false;
        private bool IsDisplayed = false;
        private bool UnlockAll = false;
        private bool SpaceStationSeen = false;
        private bool InhabitablePlanetFound = false;
        private bool AsteroidFieldFound = false;

        private Random rand;
        private Pen pen;
        private SolidBrush brush;
        private Bitmap DrawingArea;
        
        public NEA_Proj()
        {
            InitializeComponent();
            rand = new Random((int)DateTime.Now.Ticks);
            pen = new Pen(Color.White);
            brush = new SolidBrush(Color.White);
        }

        private void InitialiseDrawingArea()
        {
            Graphics g = Graphics.FromImage(DrawingArea);
            g.Clear(Color.Black);
        }

        private void LoadProj(object sender, EventArgs e)
        {
            DrawingArea = new Bitmap(800, 800, PixelFormat.Format24bppRgb);
            InitialiseDrawingArea();
            MainMenu();
        }

        private void CloseProj(object sender, FormClosedEventArgs e)
        {
            DrawingArea.Dispose();
        }

        private void PaintProj(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(DrawingArea, 0, 0, DrawingArea.Width,DrawingArea.Height);
        }

        private void MainMenu()
        {
            Graphics g = Graphics.FromImage(DrawingArea);

            g.Clear(BackColor);
            for (int i = 0; i < 100; i++)
            {
                int radius = 2;
                int PosX = rand.Next(0, this.Width);
                int PosY = rand.Next(0, this.Height);

                g.FillEllipse(brush, PosX, PosY, radius, radius);
            }
            Invalidate();
        }

        //--------------------START GAME BUTTON---------------------//
        private void SGameHover(object sender, EventArgs e)
        {
            SGame.ForeColor = Color.Gray;
        }
        private void SGameUnHover(object sender, EventArgs e)
        {
            SGame.ForeColor = Color.White;
        }
        private void SGameClick(object sender, EventArgs e)
        {
            MainTitle.Visible = false;
            SGame.Visible = false;
            Setting.Visible = false;
            Info.Visible = false;
            QGame.Visible = false;

            ShipSelectTitle.Visible = true;
            Ship1SelectBox.Visible = true;

            if (SpaceStationSeen == true || UnlockAll == true)
            {
                Ship2SelectBox.Visible = true;
            }
            else
            {
                Ship2SelectBoxLock.Visible = true;
            }

            if (InhabitablePlanetFound == true || UnlockAll == true)
            {
                Ship3SelectBox.Visible = true;
            }
            else
            {
                Ship3SelectBoxLock.Visible = true;
            }

            if (AsteroidFieldFound == true || UnlockAll == true)
            {
                Ship4SelectBox.Visible = true;
            }
            else
            {
                Ship4SelectBoxLock.Visible = true;
            }
        }

        private void Ship1SelectClick(object sender, EventArgs e)
        {
            Ship2.Visible = false;
            Ship3.Visible = false;
            Ship4.Visible = false;

            ShipSelectTitle.Visible = false;
            Ship1SelectBox.Visible = false;
            Ship2SelectBox.Visible = false; Ship2SelectBoxLock.Visible = false;
            Ship3SelectBox.Visible = false; Ship3SelectBoxLock.Visible = false;
            Ship4SelectBox.Visible = false; Ship4SelectBoxLock.Visible = false;
            
            GeneratePlanets();
            Ship1.Visible = true;
            CoordinatesText.Visible = true;
            CoordinatesPosX.Visible = true;
            CoordinatesPosY.Visible = true;
            CoordinatesComma.Visible = true;
            
            CurrentShip = 1;
        }

        private void Ship2SelectClick(object sender, EventArgs e)
        {
            Ship1.Visible = false;
            Ship3.Visible = false;
            Ship4.Visible = false;

            ShipSelectTitle.Visible = false;
            Ship1SelectBox.Visible = false;
            Ship2SelectBox.Visible = false; Ship2SelectBoxLock.Visible = false;
            Ship3SelectBox.Visible = false; Ship3SelectBoxLock.Visible = false;
            Ship4SelectBox.Visible = false; Ship4SelectBoxLock.Visible = false;

            GeneratePlanets();
            Ship2.Visible = true;
            CoordinatesText.Visible = true;
            CoordinatesPosX.Visible = true;
            CoordinatesPosY.Visible = true;
            CoordinatesComma.Visible = true;

            CurrentShip = 2;
        }

        private void Ship3SelectClick(object sender, EventArgs e)
        {
            Ship1.Visible = false;
            Ship2.Visible = false;
            Ship4.Visible = false;

            ShipSelectTitle.Visible = false;
            Ship1SelectBox.Visible = false;
            Ship2SelectBox.Visible = false; Ship2SelectBoxLock.Visible = false;
            Ship3SelectBox.Visible = false; Ship3SelectBoxLock.Visible = false;
            Ship4SelectBox.Visible = false; Ship4SelectBoxLock.Visible = false;

            GeneratePlanets();
            Ship3.Visible = true;
            CoordinatesText.Visible = true;
            CoordinatesPosX.Visible = true;
            CoordinatesPosY.Visible = true;
            CoordinatesComma.Visible = true;

            CurrentShip = 3;
        }

        private void Ship4SelectClick(object sender, EventArgs e)
        {
            Ship1.Visible = false;
            Ship2.Visible = false;
            Ship3.Visible = false;

            ShipSelectTitle.Visible = false;
            Ship1SelectBox.Visible = false;
            Ship2SelectBox.Visible = false; Ship2SelectBoxLock.Visible = false;
            Ship3SelectBox.Visible = false; Ship3SelectBoxLock.Visible = false;
            Ship4SelectBox.Visible = false; Ship4SelectBoxLock.Visible = false;

            GeneratePlanets();
            Ship4.Visible = true;
            CoordinatesText.Visible = true;
            CoordinatesPosX.Visible = true;
            CoordinatesPosY.Visible = true;
            CoordinatesComma.Visible = true;

            CurrentShip = 4;
        }

        //---------------------SETTINGS BUTTON----------------------//
        public void EnableDoubleBuffering() //This method of enabling double buffering at runtime is from https://stackoverflow.com/questions/76993/how-to-double-buffer-net-controls-on-a-form, and is by the user "Arno"
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
        public void DisableDoubleBuffering() //This method of disabling double buffering at runtime is from https://stackoverflow.com/questions/76993/how-to-double-buffer-net-controls-on-a-form, and is by the user "Arno"
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, false);
            UpdateStyles();
        }

        private void SettingHover(object sender, EventArgs e)
        {
            Setting.ForeColor = Color.Gray;
        }
        private void SettingUnHover(object sender, EventArgs e)
        {
            Setting.ForeColor = Color.White;
        }
        private void SettingClick(object sender, EventArgs e)
        {
            MainTitle.Visible = false;
            SGame.Visible = false;
            Setting.Visible = false;
            Info.Visible = false;
            QGame.Visible = false;

            SettingTitle.Visible = true;
            SettingDB.Visible = true;
            SettingRedOn.Visible = true;
            SettingSlash.Visible = true;
            SettingWhiteOff.Visible = true;
            SettingControls.Visible = true;
            SettingWhiteArrows.Visible = true;
            SettingSlash2.Visible = true;
            SettingRedWASD.Visible = true;
            SettingUA.Visible = true;
            SettingBack.Visible = true;

            if (UsingArrows == true)
            {
                SettingWhiteArrows.Visible = false;
                SettingRedWASD.Visible = false;
                SettingRedArrows.Visible = true;
                SettingWhiteWASD.Visible = true;
            }
        }

        private void SettingOnHover(object sender, EventArgs e)
        {
            SettingWhiteOn.ForeColor = Color.Gray;
        }
        private void SettingOnUnHover(object sender, EventArgs e)
        {
            SettingWhiteOn.ForeColor = Color.White;
        }
        private void SettingOnClick(object sender, EventArgs e)
        {
            SettingWhiteOn.Visible = false;
            SettingRedOff.Visible = false;
            SettingRedOn.Visible = true;
            SettingWhiteOff.Visible = true;
            EnableDoubleBuffering();
        }
        private void SettingOffHover(object sender, EventArgs e)
        {
            SettingWhiteOff.ForeColor = Color.Gray;
        }
        private void SettingOffUnHover(object sender, EventArgs e)
        {
            SettingWhiteOff.ForeColor = Color.White;
        }
        private void SettingOffClick(object sender, EventArgs e)
        {
            SettingWhiteOff.Visible = false;
            SettingRedOn.Visible = false;
            SettingRedOff.Visible = true;
            SettingWhiteOn.Visible = true;
            DisableDoubleBuffering();
        }

        private void SettingArrowsHover(object sender, EventArgs e)
        {
            SettingWhiteArrows.ForeColor = Color.Gray;
        }
        private void SettingArrowsUnHover(object sender, EventArgs e)
        {
            SettingWhiteArrows.ForeColor = Color.White;
        }
        private void SettingArrowsClick(object sender, EventArgs e)
        {
            SettingWhiteArrows.Visible = false;
            SettingRedWASD.Visible = false;
            SettingRedArrows.Visible = true;
            SettingWhiteWASD.Visible = true;
            UsingArrows = true;
        }
        private void SettingWASDHover(object sender, EventArgs e)
        {
            SettingWhiteWASD.ForeColor = Color.Gray;
        }
        private void SettingWASDUnHover(object sender, EventArgs e)
        {
            SettingWhiteWASD.ForeColor = Color.White;
        }
        private void SettingWASDClick(object sender, EventArgs e)
        {
            SettingWhiteWASD.Visible = false;
            SettingRedArrows.Visible = false;
            SettingRedWASD.Visible = true;
            SettingWhiteArrows.Visible = true;
            UsingArrows = false;
        }

        private void SettingUAHover(object sender, EventArgs e)
        {
            SettingUA.ForeColor = Color.Gray;
        }
        private void SettingUAUnHover(object sender, EventArgs e)
        {
            SettingUA.ForeColor = Color.White;
        }
        private void SettingUAClick(object sender, EventArgs e)
        {
            SettingUA.ForeColor = Color.Red;
            UnlockAll = true;
        }

        private void SettingBackHover(object sender, EventArgs e)
        {
            SettingBack.ForeColor = Color.Gray;
        }
        private void SettingBackUnHover(object sender, EventArgs e)
        {
            SettingBack.ForeColor = Color.White;
        }
        private void SettingBackClick(object sender, EventArgs e)
        {
            SettingTitle.Visible = false;
            SettingDB.Visible = false;
            SettingRedOn.Visible = false;
            SettingWhiteOn.Visible = false;
            SettingSlash.Visible = false;
            SettingWhiteOff.Visible = false;
            SettingRedOff.Visible = false;
            SettingControls.Visible = false;
            SettingWhiteArrows.Visible = false;
            SettingRedArrows.Visible = false;
            SettingSlash2.Visible = false;
            SettingRedWASD.Visible = false;
            SettingWhiteWASD.Visible = false;
            SettingUA.Visible = false;
            SettingBack.Visible = false;

            MainTitle.Visible = true;
            SGame.Visible = true;
            Setting.Visible = true;
            Info.Visible = true;
            QGame.Visible = true;
        }

        //--------------------INFORMATION BUTTON--------------------//
        private void InfoHover(object sender, EventArgs e)
        {
            Info.ForeColor = Color.Gray;
        }
        private void InfoUnHover(object sender, EventArgs e)
        {
            Info.ForeColor = Color.White;
        }
        private void InfoClick(object sender, EventArgs e)
        {
            MainTitle.Visible = false;
            SGame.Visible = false;
            Setting.Visible = false;
            Info.Visible = false;
            QGame.Visible = false;

            InfoTitle.Visible = true;
            InfoHeader.Visible = true;
            InfoLine1.Visible = true;
            InfoLine2.Visible = true;
            InfoLine3.Visible = true;
            InfoLine4.Visible = true;
            InfoLine5.Visible = true;
            InfoOutro.Visible = true;
            InfoBack.Visible = true;
        }

        private void InfoBackHover(object sender, EventArgs e)
        {
            InfoBack.ForeColor = Color.Gray;
        }
        private void InfoBackUnHover(object sender, EventArgs e)
        {
            InfoBack.ForeColor = Color.White;
        }
        private void InfoBackClick(object sender, EventArgs e)
        {
            InfoTitle.Visible = false;
            InfoHeader.Visible = false;
            InfoLine1.Visible = false;
            InfoLine2.Visible = false;
            InfoLine3.Visible = false;
            InfoLine4.Visible = false;
            InfoLine5.Visible = false;
            InfoOutro.Visible = false;
            InfoBack.Visible = false;

            MainTitle.Visible = true;
            SGame.Visible = true;
            Setting.Visible = true;
            Info.Visible = true;
            QGame.Visible = true;
        }

        //---------------------QUIT GAME BUTTON---------------------//
        private void QGameHover(object sender, EventArgs e)
        {
            QGame.ForeColor = Color.Gray;
        }
        private void QGameUnHover(object sender, EventArgs e)
        {
            QGame.ForeColor = Color.White;
        }
        private void QGameClick(object sender, EventArgs e)
        {
            Close();
        }

        //--------------------PLANET GENERATION---------------------//
        private void GeneratePlanets()
        {
            Graphics g = Graphics.FromImage(DrawingArea);
            brush.Color = Color.White;
            g.Clear(BackColor);

            int[,] PlanetsInfo = new int[10,6];
            
            int SpaceStationExists = rand.Next(0, 101);                                         
            if (SpaceStationExists == 0 || SpaceStationExists == 1 || SpaceStationExists == 2) // Means a 3% chance because SpaceStationExists is a random integer between 0 and 100
            {
                SpaceStation.Visible = true;
                SpaceStationSeen = true; // Keeps track of if a space station has been seen before to allow the user to pick the corresponding ship or not
                if (SpaceStationCounter == 0) // Variable used to make sure the "Ship Unlocked!" text is only displayed once
                {
                    ShipUnlockedTitle.Visible = true;
                    SpaceStationCounter += 1;
                }
                tmrUnlocked.Start(); // Timer that removes the "Ship Unlocked!" text after 3 seconds
            }                                                                                   

            List<Buffer> buffers = new List<Buffer>();
            List<Buffer> GroupX = new List<Buffer>();
            List<Buffer> GroupY = new List<Buffer>();

            for (int i = 0; i < 100; i++) // Generates 100 white dots each 2 pixels in size to act as stars
            {
                int radius = 2;
                int PosX = rand.Next(0, this.Width);
                int PosY = rand.Next(0, this.Height);

                g.FillEllipse(brush, PosX, PosY, radius, radius);
            }

            for (int i = 0; i < 10; i++) // Generates 10 circles with random attributes that act as planets
            {
                int radius = rand.Next(30, 50);
                int PosX = rand.Next(0, this.Width);
                int PosY = rand.Next(0, this.Height);

                buffers.Add(new Buffer { X = PosX, Y = PosY }); //
                GroupX.Add(new Buffer { X = buffers[i].X });    // List elements used to keep track of planets after they have gone off screen
                GroupY.Add(new Buffer { Y = buffers[i].Y });    //

                for (int n = 1; n < GroupX.Count; n++) // Algorithm for collision detection, not perfect but makes it far less likely that a circle will generate on top of another one
                {
                    for (int o = 0; o < GroupX.Count; o++)
                    {
                        int c = 0;
                        if (GroupX[i].X == GroupX[i - n + c].X)
                        {
                            radius = rand.Next(40, 60);
                            PosX = rand.Next(0, this.Width);
                            PosY = rand.Next(0, this.Height);
                            c++;
                        }
                        else
                        {
                            c++;
                        }
                    }
                }

                int color1 = rand.Next(25, 255);
                int color2 = rand.Next(25, 255);
                int color3 = rand.Next(25, 255);
                brush.Color = Color.FromArgb(color1, color2, color3);

                g.FillEllipse(brush, PosX, PosY, radius, radius);

                int[] PlanetInfo = { radius, PosX, PosY, color1, color2, color3 };
                for (int j = 0; j < PlanetInfo.Count(); j++) // Stores the attributes of each planet inside a 2d array, where each planet has it's own array
                {
                    PlanetsInfo[i,j] = PlanetInfo[j];
                }
            }

            bool PositionExists = false;
            for (int p = 0; p < positions.Count; p++) // Checks whether the player has been to the current grid position before or not, to avoid adding duplicate attributes to the list below
            {
                bool ContainsX = positions[p].X == CurrentGridPosition[0];
                bool ContainsY = positions[p].Y == CurrentGridPosition[1];

                if (ContainsX == true && ContainsY == true)
                {
                    PositionExists = true;
                }
            }

            if (PositionExists == false)
            {
                positions.Add(new Position() { X = CurrentGridPosition[0], Y = CurrentGridPosition[1], Planets = PlanetsInfo }); // A list called positions that stores the grid position and attributes of planets in that position so they can be re-generated upon revisiting
            }
            else // getting values from the positions list to be used in the GenerateExistingPlanets function that allows the planets to be re-generated
            {
                int index = positions.FindIndex(p => p.X == CurrentGridPosition[0] && p.Y == CurrentGridPosition[1]);
                g.Clear(BackColor);
                for (int p = 0; p < 10; p++)
                {
                    int FuncRadius = positions[index].Planets[p, 0];
                    int FuncPosX = positions[index].Planets[p, 1];
                    int FuncPosY = positions[index].Planets[p, 2];
                    int FuncColor1 = positions[index].Planets[p, 3];
                    int FuncColor2 = positions[index].Planets[p, 4];
                    int FuncColor3 = positions[index].Planets[p, 5];

                    GenerateExistingPlanets(FuncRadius, FuncPosX, FuncPosY, FuncColor1, FuncColor2, FuncColor3);
                }
            }
            
            int AsteroidFieldExists = rand.Next(0, 101);
            if (AsteroidFieldExists == 0 || AsteroidFieldExists == 1) // Means a 2% chance because AsteroidFieldExists is a random integer between 0 and 100
            {
                GenerateAsteroidField();
                if (AsteroidCounter == 0) // Variable used to make sure the "Ship Unlocked!" text is only displayed once
                {
                    ShipUnlockedTitle.Visible = true;
                    AsteroidCounter += 1;
                }
                tmrUnlocked.Start();
                AsteroidFieldFound = true; // Keeps track of if an asteroid field has been seen before to allow the user to pick the corresponding ship or not
            }
            Invalidate();
        }

        //----------------EXISTING PLANET GENERATION----------------//
        private void GenerateExistingPlanets(int radius, int PosX, int PosY, int color1, int color2, int color3) // Function that allows planets to be re-generated
        {
            Graphics g = Graphics.FromImage(DrawingArea);
            brush.Color = Color.White;

            for (int i = 0; i < 10; i++) // Function also includes the stars generation, however the positions of these will be different to the first time it is generated as they are not stored anywhere for optimisation
            {
                int StarRadius = 2;
                int StarPosX = rand.Next(0, this.Width);
                int StarPosY = rand.Next(0, this.Height);
                g.FillEllipse(brush, StarPosX, StarPosY, StarRadius, StarRadius);
            }

            brush.Color = Color.FromArgb(color1, color2, color3);

            g.FillEllipse(brush, PosX, PosY, radius, radius);
        }

        //----------------ASTEROID FIELD GENERATION-----------------//
        private void GenerateAsteroidField() // Creates 10 asteroids
        {
            Graphics g = Graphics.FromImage(DrawingArea);
            brush.Color = Color.White;
            g.Clear(BackColor);

            for (int i = 0; i < 100; i++)
            {
                int StarRadius = 2;
                int StarPosX = rand.Next(0, this.Width);
                int StarPosY = rand.Next(0, this.Height);
                g.FillEllipse(brush, StarPosX, StarPosY, StarRadius, StarRadius);
            }

            for (int a = 0; a < 10; a++) // Makes each asteroid a random shade of grey from four different shades
            {
                int color = rand.Next(0, 4);
                if (color == 0)
                {
                    brush.Color = Color.FromArgb(192, 192, 192);
                }
                else if (color == 1)
                {
                    brush.Color = Color.FromArgb(169, 169, 169);
                }
                else if (color == 2)
                {
                    brush.Color = Color.FromArgb(128, 128, 128);
                }
                else
                {
                    brush.Color = Color.FromArgb(105, 105, 105);
                }

                int p2StepX = rand.Next(15, 30);
                int p2StepY = rand.Next(15, 30);
                int p4StepX = rand.Next(15, 30);
                int p6StepX = rand.Next(15, 30);
                int p6StepY = rand.Next(15, 30);
                int p8StepX = rand.Next(15, 30);
                int p8StepY = rand.Next(15, 30);
                int p10StepX = rand.Next(15, 30);

                Point p1 = new Point(rand.Next(0, DrawingArea.Width), rand.Next(0, DrawingArea.Height)); // Creating points to be used to create the bottom half of a polygon that has variance in the shape of it's sides to give the impression of a rough non-circular asteroid
                Point p2 = new Point(p1.X + (p2StepX / rand.Next(2,5)), p1.Y + (p2StepY / rand.Next(2, 5)));
                Point p3 = new Point(p2.X + p2StepX, p2.Y + p2StepY);
                Point p4 = new Point(p3.X + (p4StepX / rand.Next(2, 5)), p3.Y);
                Point p5 = new Point(p4.X + p4StepX, p4.Y);
                Point p6 = new Point(p5.X + (p6StepX / rand.Next(2, 5)), p5.Y - (p6StepY / rand.Next(2, 5)));
                Point p7 = new Point(p6.X + p6StepX, p6.Y - p6StepY);
                Point p8 = new Point(p7.X - (p8StepX / rand.Next(2, 5)), p7.Y - (p8StepY / rand.Next(2, 5)));
                Point p9 = new Point(p8.X - p8StepX, p8.Y - p8StepY);
                Point p10 = new Point(p9.X - (p10StepX / rand.Next(2, 5)), p9.Y);
                Point p11 = new Point(p10.X - p10StepX, p10.Y);

                Point[] AsteroidBot = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 }; // Adding points to an array for the FillPolygon function

                int a2StepX = rand.Next(15, 30);
                int a2StepY = rand.Next(15, 30);
                int a4StepX = rand.Next(15, 30);
                int a6StepX = rand.Next(15, 30);
                int a6StepY = rand.Next(15, 30);
                int a8StepX = rand.Next(15, 30);
                int a8StepY = rand.Next(15, 30);
                int a10StepX = rand.Next(15, 30);

                Point a1 = new Point(p1.X, p1.Y); // Creating points this time for the top half of the asteroid
                Point a2 = new Point(a1.X + (a2StepX / rand.Next(2, 5)), a1.Y - (a2StepY / rand.Next(2, 5)));
                Point a3 = new Point(a2.X + a2StepX, a2.Y - a2StepY);
                Point a4 = new Point(a3.X + (a4StepX / rand.Next(2, 5)), a3.Y);
                Point a5 = new Point(a4.X + a4StepX, a4.Y);
                Point a6 = new Point(a5.X + (a6StepX / rand.Next(2, 5)), a5.Y + (a6StepY / rand.Next(2, 5)));
                Point a7 = new Point(a6.X + a6StepX, a6.Y + a6StepY);
                Point a8 = new Point(a7.X - (a8StepX / rand.Next(2, 5)), a7.Y + (a8StepY / rand.Next(2, 5)));
                Point a9 = new Point(a8.X - a8StepX, a8.Y + a8StepY);
                Point a10 = new Point(a9.X - (a10StepX / rand.Next(2, 5)), a9.Y);
                Point a11 = new Point(a10.X - a10StepX, a10.Y);
                
                Point[] AsteroidTop = { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 }; // Adding points to the second array

                g.FillPolygon(brush, AsteroidBot); // Generating the top and bottom of the asteroid to make a whole one
                g.FillPolygon(brush, AsteroidTop); // Generating the top and bottom separately allows for a more variable shape as it will not be symmetrical and can be very different in size
            }
        }

        //-----------------------NEXT SCREEN------------------------//
        private void NextScreen(object sender, EventArgs e)
        {
            if (Ship1.Location.Y == 40 || Ship1.Location.Y == 41 || Ship1.Location.Y == 39 || Ship1.Location.Y == 42 || Ship1.Location.Y == 38 || Ship1.Location.Y == 43 || Ship1.Location.Y == 37 ||
                Ship2.Location.Y == 40 || Ship2.Location.Y == 41 || Ship2.Location.Y == 39 || Ship2.Location.Y == 42 || Ship2.Location.Y == 38 || Ship2.Location.Y == 43 || Ship2.Location.Y == 37 ||
                Ship3.Location.Y == 40 || Ship3.Location.Y == 41 || Ship3.Location.Y == 39 || Ship3.Location.Y == 42 || Ship3.Location.Y == 38 || Ship3.Location.Y == 43 || Ship3.Location.Y == 37 ||
                Ship4.Location.Y == 40 || Ship4.Location.Y == 41 || Ship4.Location.Y == 39 || Ship4.Location.Y == 42 || Ship4.Location.Y == 38 || Ship4.Location.Y == 43 || Ship4.Location.Y == 37) // Checking for hitbox collision with the edge of the screen
            {
                CurrentGridPosition[1] += 1;
                SpaceStation.Visible = false;

                if (CurrentShip == 1)
                {
                    Ship1.Visible = false;
                    Ship1.Top += 640; // Moves the ship to the other side of the screen to create the illusion of the camera moving
                    GeneratePlanets();
                    Ship1.Visible = true;
                }
                else if (CurrentShip == 2)
                {
                    Ship2.Visible = false;
                    Ship2.Top += 640;
                    GeneratePlanets();
                    Ship2.Visible = true;
                }
                else if (CurrentShip == 3)
                {
                    Ship3.Visible = false;
                    Ship3.Top += 640;
                    GeneratePlanets();
                    Ship3.Visible = true;
                }
                else
                {
                    Ship4.Visible = false;
                    Ship4.Top += 640;
                    GeneratePlanets();
                    Ship4.Visible = true;
                }
            }
            if (Ship1.Location.X == 40 || Ship1.Location.X == 41 || Ship1.Location.X == 39 || Ship1.Location.X == 42 || Ship1.Location.X == 38 || Ship1.Location.X == 43 || Ship1.Location.X == 37 ||
                Ship2.Location.X == 40 || Ship2.Location.X == 41 || Ship2.Location.X == 39 || Ship2.Location.X == 42 || Ship2.Location.X == 38 || Ship2.Location.X == 43 || Ship2.Location.X == 37 ||
                Ship3.Location.X == 40 || Ship3.Location.X == 41 || Ship3.Location.X == 39 || Ship3.Location.X == 42 || Ship3.Location.X == 38 || Ship3.Location.X == 43 || Ship3.Location.X == 37 ||
                Ship4.Location.X == 40 || Ship4.Location.X == 41 || Ship4.Location.X == 39 || Ship4.Location.X == 42 || Ship4.Location.X == 38 || Ship4.Location.X == 43 || Ship4.Location.X == 37)
            {
                CurrentGridPosition[0] -= 1;
                SpaceStation.Visible = false;

                if (CurrentShip == 1)
                {
                    Ship1.Visible = false;
                    Ship1.Left += 640;
                    GeneratePlanets();
                    Ship1.Visible = true;
                }
                else if (CurrentShip == 2)
                {
                    Ship2.Visible = false;
                    Ship2.Left += 640;
                    GeneratePlanets();
                    Ship2.Visible = true;
                }
                else if (CurrentShip == 3)
                {
                    Ship3.Visible = false;
                    Ship3.Left += 640;
                    GeneratePlanets();
                    Ship3.Visible = true;
                }
                else
                {
                    Ship4.Visible = false;
                    Ship4.Left += 640;
                    GeneratePlanets();
                    Ship4.Visible = true;
                }
            }
            if (Ship1.Location.Y == 695 || Ship1.Location.Y == 694 || Ship1.Location.Y == 696 || Ship1.Location.Y == 693 || Ship1.Location.Y == 697 || Ship1.Location.Y == 692 || Ship1.Location.Y == 698 ||
                Ship2.Location.Y == 695 || Ship2.Location.Y == 694 || Ship2.Location.Y == 696 || Ship2.Location.Y == 693 || Ship2.Location.Y == 697 || Ship2.Location.Y == 692 || Ship2.Location.Y == 698 ||
                Ship3.Location.Y == 695 || Ship3.Location.Y == 694 || Ship3.Location.Y == 696 || Ship3.Location.Y == 693 || Ship3.Location.Y == 697 || Ship3.Location.Y == 692 || Ship3.Location.Y == 698 ||
                Ship4.Location.Y == 695 || Ship4.Location.Y == 694 || Ship4.Location.Y == 696 || Ship4.Location.Y == 693 || Ship4.Location.Y == 697 || Ship4.Location.Y == 692 || Ship4.Location.Y == 698)
            {
                CurrentGridPosition[1] -= 1;
                SpaceStation.Visible = false;

                if (CurrentShip == 1)
                {
                    Ship1.Visible = false;
                    Ship1.Top -= 640;
                    GeneratePlanets();
                    Ship1.Visible = true;
                }
                else if (CurrentShip == 2)
                {
                    Ship2.Visible = false;
                    Ship2.Top -= 640;
                    GeneratePlanets();
                    Ship2.Visible = true;
                }
                else if (CurrentShip == 3)
                {
                    Ship3.Visible = false;
                    Ship3.Top -= 640;
                    GeneratePlanets();
                    Ship3.Visible = true;
                }
                else
                {
                    Ship4.Visible = false;
                    Ship4.Top -= 640;
                    GeneratePlanets();
                    Ship4.Visible = true;
                }
            }
            if (Ship1.Location.X == 695 || Ship1.Location.X == 694 || Ship1.Location.X == 696 || Ship1.Location.X == 693 || Ship1.Location.X == 697 || Ship1.Location.X == 692 || Ship1.Location.X == 698 ||
                Ship2.Location.X == 695 || Ship2.Location.X == 694 || Ship2.Location.X == 696 || Ship2.Location.X == 693 || Ship2.Location.X == 697 || Ship2.Location.X == 692 || Ship2.Location.X == 698 ||
                Ship3.Location.X == 695 || Ship3.Location.X == 694 || Ship3.Location.X == 696 || Ship3.Location.X == 693 || Ship3.Location.X == 697 || Ship3.Location.X == 692 || Ship3.Location.X == 698 ||
                Ship4.Location.X == 695 || Ship4.Location.X == 694 || Ship4.Location.X == 696 || Ship4.Location.X == 693 || Ship4.Location.X == 697 || Ship4.Location.X == 692 || Ship4.Location.X == 698)
            {
                CurrentGridPosition[0] += 1;
                SpaceStation.Visible = false;

                if (CurrentShip == 1)
                {
                    Ship1.Visible = false;
                    Ship1.Left -= 640;
                    GeneratePlanets();
                    Ship1.Visible = true;
                }
                else if (CurrentShip == 2)
                {
                    Ship2.Visible = false;
                    Ship2.Left -= 640;
                    GeneratePlanets();
                    Ship2.Visible = true;
                }
                else if (CurrentShip == 3)
                {
                    Ship3.Visible = false;
                    Ship3.Left -= 640;
                    GeneratePlanets();
                    Ship3.Visible = true;
                }
                else
                {
                    Ship4.Visible = false;
                    Ship4.Left -= 640;
                    GeneratePlanets();
                    Ship4.Visible = true;
                }
            }
            CoordinatesPosX.Text = System.Convert.ToString(CurrentGridPosition[0]); // Updating values for the coordinates in the top left
            CoordinatesPosY.Text = System.Convert.ToString(CurrentGridPosition[1]);
        }

        //----------------------PLANET DETAILS----------------------//
        private void DisplayPlanetDetails() // Creates a panel in the bottom right that shows details of the planet you are over
        {
            Graphics g = Graphics.FromImage(DrawingArea);

            if (IsDisplayed == false)
            {
                pen.Color = Color.Red;
                if (CurrentShip == 1) // Creates the red circle around the ship when space is pressed
                {
                    g.DrawEllipse(pen, Ship1.Location.X - 20, Ship1.Location.Y - 20, 70, 70);
                }
                else if (CurrentShip == 2)
                {
                    g.DrawEllipse(pen, Ship2.Location.X - 20, Ship2.Location.Y - 20, 70, 70);
                }
                else if (CurrentShip == 3)
                {
                    g.DrawEllipse(pen, Ship3.Location.X - 20, Ship3.Location.Y - 20, 70, 70);
                }
                else
                {
                    g.DrawEllipse(pen, Ship4.Location.X - 20, Ship4.Location.Y - 20, 70, 70);
                }

                int temp = rand.Next(-100, 101); // Calculating and displaying all of the attributes of the planet
                TemperatureText.Text = System.Convert.ToString(temp + "°C");

                int Percentage = rand.Next(0, 101);
                int LandPercentage = 100 - Percentage;
                int WaterPercentage = 100 - LandPercentage;
                FoliageText.Text = System.Convert.ToString(LandPercentage + "%");
                WaterText.Text = System.Convert.ToString(WaterPercentage + "%");

                int minerals = rand.Next(0, 2);
                int gases = rand.Next(0, 2);
                if (minerals == 1)
                {
                    MineralsText.Text = "Present";
                }
                else
                {
                    MineralsText.Text = "Not Present";
                }
                if (gases == 1)
                {
                    GasesText.Text = "Present";
                }
                else
                {
                    GasesText.Text = "Not Present";
                }

                int population = rand.Next(0, 12);
                if (population <= 10)
                {
                    PopulationText.Text = System.Convert.ToString(population + " Species");
                }
                else
                {
                    PopulationText.Text = "10+ Species";
                }

                if ((LandPercentage >= 30 && LandPercentage <= 70) && (WaterPercentage >= 30 && WaterPercentage <= 70) && (temp <= 30 && temp >= -30) && minerals == 1 && gases == 1 && population >= 3) //Checking whether requirements are met for an inhabitable planet
                {
                    InhabitableText.Text = "Yes";
                    InhabitableText.ForeColor = Color.Green;
                    if (InhabitableCounter == 0) // Variable used to make sure the "Ship Unlocked!" text is only displayed once
                    {
                        ShipUnlockedTitle.Visible = true;
                        InhabitableCounter += 1;
                    }
                    tmrUnlocked.Start();
                    InhabitablePlanetFound = true; // Keeps track of if an inhabitable planet has been found before to allow the user to pick the corresponding ship or not
                }
                else
                {
                    InhabitableText.Text = "No";
                    InhabitableText.ForeColor = Color.Red;
                }

                TemperatureText.Visible = true;
                FoliageText.Visible = true;
                WaterText.Visible = true;
                MineralsText.Visible = true;
                GasesText.Visible = true;
                PopulationText.Visible = true;
                InhabitableText.Visible = true;
                PlanetInfoPanel.Visible = true;
                IsDisplayed = true; // This variable changes the effect of space the next time it is pressed
            }
            else
            {
                pen.Color = Color.Black;
                if (CurrentShip == 1) // Creates a black circle around the ship to remove the red circle
                {
                    g.DrawEllipse(pen, Ship1.Location.X - 20, Ship1.Location.Y - 20, 70, 70);
                }
                else if (CurrentShip == 2)
                {
                    g.DrawEllipse(pen, Ship2.Location.X - 20, Ship2.Location.Y - 20, 70, 70);
                }
                else if (CurrentShip == 3)
                {
                    g.DrawEllipse(pen, Ship3.Location.X - 20, Ship3.Location.Y - 20, 70, 70);
                }
                else
                {
                    g.DrawEllipse(pen, Ship4.Location.X - 20, Ship4.Location.Y - 20, 70, 70);
                }

                TemperatureText.Visible = false;
                FoliageText.Visible = false;
                WaterText.Visible = false;
                MineralsText.Visible = false;
                GasesText.Visible = false;
                PopulationText.Visible = false;
                InhabitableText.Visible = false;
                PlanetInfoPanel.Visible = false;
                IsDisplayed = false; // The effect of space is now back to it's previous version, meaning it alternates every time
            }
            pen.Color = Color.White;
            Invalidate();
        }

        //-------------DISPLAY AND REMOVE UNLOCKED TEXT-------------//
        private void tmrUnlockedTick(object sender, EventArgs e)
        {
            ShipUnlockedTitle.Visible = false;
            tmrUnlocked.Stop();
        }

        //-----------------------MOVE PLAYER------------------------//
        private void tmrForwardTick(object sender, EventArgs e) // Timers are used for smooth movement as a workaround for minimal built in movement in Windows Forms Applications
        {
            if (CurrentShip == 1)
            {
                Ship1.Top -= 2;
            }
            else if (CurrentShip == 2)
            {
                Ship2.Top -= 2;
            }
            else if (CurrentShip == 3)
            {
                Ship3.Top -= 2;
            }
            else
            {
                Ship4.Top -= 2;
            }
        }
        private void tmrBackwardTick(object sender, EventArgs e)
        {
            if (CurrentShip == 1)
            {
                Ship1.Top += 2;
            }
            else if (CurrentShip == 2)
            {
                Ship2.Top += 2;
            }
            else if (CurrentShip == 3)
            {
                Ship3.Top += 2;
            }
            else
            {
                Ship4.Top += 2;
            }
        }
        private void tmrRightTick(object sender, EventArgs e)
        {
            if (CurrentShip == 1)
            {
                Ship1.Left += 2;
            }
            else if (CurrentShip == 2)
            {
                Ship2.Left += 2;
            }
            else if (CurrentShip == 3)
            {
                Ship3.Left += 2;
            }
            else
            {
                Ship4.Left += 2;
            }
        }
        private void tmrLeftTick(object sender, EventArgs e)
        {
            if (CurrentShip == 1)
            {
                Ship1.Left -= 2;
            }
            else if (CurrentShip == 2)
            {
                Ship2.Left -= 2;
            }
            else if (CurrentShip == 3)
            {
                Ship3.Left -= 2;
            }
            else
            {
                Ship4.Left -= 2;
            }
        }
        private void Ship1KeyDown(object sender, KeyEventArgs e)
        {
            Image RotShip1 = Ship1.Image;
            Image RotShip2 = Ship2.Image;
            Image RotShip3 = Ship3.Image;
            Image RotShip4 = Ship4.Image;

            if (e.KeyCode == Keys.Escape) // Displays the main menu
            {
                brush.Color = Color.White;
                MainMenu();
                Ship1.Visible = false;
                Ship2.Visible = false;
                Ship3.Visible = false;
                Ship4.Visible = false;
                SpaceStation.Visible = false;
                CoordinatesText.Visible = false;
                CoordinatesPosX.Visible = false;
                CoordinatesPosY.Visible = false;
                CoordinatesComma.Visible = false;
                ShipUnlockedTitle.Visible = false;

                MainTitle.Visible = true;
                SGame.Visible = true;
                Setting.Visible = true;
                Info.Visible = true;
                QGame.Visible = true;
            }

            if (e.KeyCode == Keys.Space) // Displays planet details and the red circle, alternatively removes details and red circle if they are displayed already
            {
                DisplayPlanetDetails();
            }

            if (UsingArrows == false) // Setting that determines whether the user uses arrow keys or WASD to move, false means WASD
            {
                if (e.KeyCode == Keys.W)
                {
                    if (CurrentRotation == 0) // Variable that keeps track of the ship's rotation so it can be adjusted whenever the player presses a direction
                    {
                        CurrentRotation = 0;
                    }
                    else if (CurrentRotation == 90)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 0;
                    }
                    else if (CurrentRotation == 180)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 0;
                    }
                    else
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 0;
                    }
                    tmrForward.Start();
                }
                if (e.KeyCode == Keys.S)
                {
                    if (CurrentRotation == 0)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 180;
                    }
                    else if (CurrentRotation == 90)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 180;
                    }
                    else if (CurrentRotation == 180)
                    {
                        CurrentRotation = 180;
                    }
                    else
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 180;
                    }
                    tmrBackward.Start();
                }
                if (e.KeyCode == Keys.D)
                {
                    if (CurrentRotation == 0)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 90;
                    }
                    else if (CurrentRotation == 90)
                    {
                        CurrentRotation = 90;
                    }
                    else if (CurrentRotation == 180)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 90;
                    }
                    else
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 90;
                    }
                    tmrRight.Start();
                }
                if (e.KeyCode == Keys.A)
                {
                    if (CurrentRotation == 0)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 270;
                    }
                    else if (CurrentRotation == 90)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 270;
                    }
                    else if (CurrentRotation == 180)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 270;
                    }
                    else
                    {
                        CurrentRotation = 270;
                    }
                    tmrLeft.Start();
                }
            }
            else // Means that this is for when the user has chosen to play with arrow keys
            {
                if (e.KeyCode == Keys.Up)
                {
                    if (CurrentRotation == 0)
                    {
                        CurrentRotation = 0;
                    }
                    else if (CurrentRotation == 90)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 0;
                    }
                    else if (CurrentRotation == 180)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 0;
                    }
                    else
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 0;
                    }
                    tmrForward.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (CurrentRotation == 0)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 180;
                    }
                    else if (CurrentRotation == 90)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 180;
                    }
                    else if (CurrentRotation == 180)
                    {
                        CurrentRotation = 180;
                    }
                    else
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 180;
                    }
                    tmrBackward.Start();
                }
                if (e.KeyCode == Keys.Right)
                {
                    if (CurrentRotation == 0)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 90;
                    }
                    else if (CurrentRotation == 90)
                    {
                        CurrentRotation = 90;
                    }
                    else if (CurrentRotation == 180)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 90;
                    }
                    else
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 90;
                    }
                    tmrRight.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (CurrentRotation == 0)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        CurrentRotation = 270;
                    }
                    else if (CurrentRotation == 90)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        CurrentRotation = 270;
                    }
                    else if (CurrentRotation == 180)
                    {
                        RotShip1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        RotShip4.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        CurrentRotation = 270;
                    }
                    else
                    {
                        CurrentRotation = 270;
                    }
                    tmrLeft.Start();
                }
            }
        }
        private void Ship1KeyUp(object sender, KeyEventArgs e) // Stops the timers and therefore movement of the ship when the player lets go of a key
        {
            if (UsingArrows == false)
            {
                if (e.KeyCode == Keys.W)
                {
                    tmrForward.Stop();
                }
                if (e.KeyCode == Keys.S)
                {
                    tmrBackward.Stop();
                }
                if (e.KeyCode == Keys.D)
                {
                    tmrRight.Stop();
                }
                if (e.KeyCode == Keys.A)
                {
                    tmrLeft.Stop();
                }
            }
            else
            {
                if (e.KeyCode == Keys.Up)
                {
                    tmrForward.Stop();
                }
                if (e.KeyCode == Keys.Down)
                {
                    tmrBackward.Stop();
                }
                if (e.KeyCode == Keys.Right)
                {
                    tmrRight.Stop();
                }
                if (e.KeyCode == Keys.Left)
                {
                    tmrLeft.Stop();
                }
            }
        }
    }

    public class Buffer // Used for planet collision detection upon generation
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Position // Used to keep track of planets after they have been generated
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[,] Planets { get; set; }
    }
}
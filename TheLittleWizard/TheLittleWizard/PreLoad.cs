using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TheLittleWizard
{
    class PreLoad
    {
        #region Field
        ContentManager content;
        SpriteBatch spriteBatch;

        private static Texture2D wizard;
        private static Texture2D enemy;
        private static Texture2D tree;
        private static Texture2D regularTower;
        private static Texture2D iceTower;
        private static Texture2D regularTowerKey;
        private static Texture2D iceTowerKey;
        private static Texture2D ground;
        private static Texture2D river;
        private static Texture2D portal;
        private static Texture2D grass;
        #endregion

        #region Properties
        public static Texture2D Wizard
        {
            get
            {
                return PreLoad.wizard;
            }
        }
        public static Texture2D Enemy
        {
            get
            {
                return PreLoad.enemy;
            }
        }
        public static Texture2D Tree
        {
            get
            {
                return PreLoad.tree;
            }
        }
        public static Texture2D RegularTower
        {
            get
            {
                return PreLoad.regularTower;
            }
        }
        public static Texture2D IceTower
        {
            get
            {
                return PreLoad.iceTower;
            }
        }
        public static Texture2D RegularTowerKey
        {
            get
            {
                return PreLoad.regularTowerKey;
            }
        }
        public static Texture2D IceTowerKey
        {
            get
            {
                return PreLoad.iceTowerKey;
            }
        }
        public static Texture2D Ground
        {
            get
            {
                return PreLoad.ground;
            }
        }
        public static Texture2D River
        {
            get
            {
                return PreLoad.river;
            }
        }
        public static Texture2D Portal
        {
            get
            {
                return PreLoad.portal;
            }
        }
        public static Texture2D Grass
        {
            get
            {
                return grass;
            }

            set
            {
                grass = value;
            }
        }
        #endregion

        /// <summary>
        /// Loads all the content ahead, instead of loading them for each object, objects references to the statics set here.
        /// </summary>
        /// <param name="content"></param>
        public static void LoadContent(ContentManager content)
        {
            content.RootDirectory = "Content";

            wizard = content.Load<Texture2D>(@"Texture\Wizard_Young");
            enemy = content.Load<Texture2D>(@"Texture\Monster");
            tree = content.Load<Texture2D>(@"Texture\Tree");
            regularTower = content.Load<Texture2D>(@"Texture\Tower");
            iceTower = content.Load<Texture2D>(@"Texture\MagicTower");
            regularTowerKey = content.Load<Texture2D>(@"Texture\TowerKey");
            iceTowerKey = content.Load<Texture2D>(@"Texture\MagicKey");
            ground = content.Load<Texture2D>(@"Texture\Path");
            river = content.Load<Texture2D>(@"Texture\Water");
            portal = content.Load<Texture2D>(@"Texture\Portal");
            grass = content.Load<Texture2D>(@"Texture\Grass");
        }
    }
}

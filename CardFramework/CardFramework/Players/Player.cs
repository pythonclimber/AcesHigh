namespace CardFramework {
    public abstract class Player {
        #region Public Properties

        public bool IsComputer { get; protected set; }

        public int Score { get; internal set; }

        #endregion

        #region Construction

        protected Player() : this(false, 0) {

        }

        /// <summary>
        /// Creates new PlayerData object, all other fields will be initialized by caller.
        /// </summary>
        protected Player(bool isComp, int score) {
            IsComputer = IsComputer;
            Score = score;
            Hand = new List<Card>();
        }

        #endregion

        public abstract string DisplayHand();
    }
}

using System;

namespace ThunderRoad
{
	[Serializable]
	public abstract class ItemStat<T> : IStats<T>
	{
        public string name;

        public abstract T GetValue();
        protected ItemStat(string name)
        {
            this.name = name;
        }
	}

	public class ItemStatFloat : ItemStat<float>
	{
		public float value;
		public ItemStatFloat(string name, float value) : base(name)
		{
			this.value = value;
		}

		public override float GetValue()
		{
			return value;
		}

	}

	public class ItemStatInt : ItemStat<int>
	{
		public int value;
        //shows with stars by default
        public bool useStarIcons = true;
		public ItemStatInt(string name, int value) : base(name)
		{
			this.value = value;
		}

		public override int GetValue()
		{
			return value;
		}

	}

	public class ItemStatString : ItemStat<string>
	{
		public string value;
		public ItemStatString(string name, string value) : base(name)
		{
			this.value = value;
		}
       
		public override string GetValue()
		{
			return value;
		}

	}

	public class ItemStatBool : ItemStat<bool>
	{
		public bool value;
		public ItemStatBool(string name, bool value) : base(name)
		{
			this.value = value;
		}
      
		public override bool GetValue()
		{
			return value;
		}

	}
}

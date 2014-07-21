using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Blender
{
	/// <summary>
	/// This class represents array type
	/// </summary>
	public class BlendArrayType : IBlendType
	{
		#region propertis

		public virtual int ArrayDimension
		{
			get
			{
				return m_dimCountArray != null
					? m_dimCountArray.Length
					: 0;
			}
		}

		public virtual bool IsArray
		{
			get
			{
				return ArrayDimension != 0;
			}
		}

		/// <summary>
		/// get a pre-qualified type
		/// </summary>
		protected IBlendType m_baseType;
		public virtual IBlendType BaseType
		{
			get
			{
				return m_baseType;
			}
		}

		/// <summary>
		/// get a name of type
		/// </summary>
		public virtual string Name
		{
			get
			{
				string str = m_baseType.ToString();

				// process an array dimension
				if (m_dimCountArray != null)
				{
					for (int indexDim = 0; indexDim < ArrayDimension; ++indexDim)
					{
						str += ("[" + m_dimCountArray[indexDim] + "]");
					}
				}

				return str;
			}
		}


		#endregion // properties

		/// <summary>
		/// Constructor for a full-spec 
		/// </summary>
		/// <param name="type">base type</param>
		/// <param name="dim1Count">first dimension count for array</param>
		/// <param name="dim2Count">second dimension count for array</param>
		public BlendArrayType(IBlendType type, int dim1Count, int dim2Count)
		{
			Debug.Assert(type != null, "type must not be null");

			m_baseType = type;

			// Build array 
			int dimension = dim1Count == 0
				? 0
				: dim2Count == 0
					? 1
					: 2;
			if (dimension != 0)
			{
				m_dimCountArray = new int[dimension];
				var dimCountArray = new int[] { dim1Count, dim2Count };
				for (int index = 0; index < m_dimCountArray.Length; ++index)
				{
					m_dimCountArray[index] = dimCountArray[index];
				}
			}

		}

		/// <summary>
		/// Constructor for a full-spec 
		/// </summary>
		/// <param name="type">base type</param>
		/// <param name="dimCountArray">array of size</param>
		public BlendArrayType(IBlendType type, int[] dimCountArray)
		{
			Debug.Assert(type != null, "type must not be null");

			m_baseType = type;
			if (dimCountArray.Length != 0)
			{
				m_dimCountArray = new int[dimCountArray.Length];
				dimCountArray.CopyTo(m_dimCountArray, 0);
			}
		}

		/// <summary>
		/// get size of this type
		/// </summary>
		/// <returns>size [byte]</returns>
		public virtual int SizeOf()
		{
			int size = m_baseType.SizeOf();

			int elemCount = 1;
			if (m_dimCountArray != null)
			{
				for (int indexDim = 0; indexDim < ArrayDimension; ++indexDim)
				{
					elemCount *= m_dimCountArray[indexDim];
				}
			}

			return size * elemCount;
		}

		public override String ToString() 
		{
			return Name;
		}

		public virtual bool Equals(IBlendType type)
		{
			var qualified = type as BlendArrayType;
			if (qualified == null)
			{
				// type unmatched
				return false;
			}

			return !m_baseType.Equals(qualified.m_baseType)
				? false
				: !_Equals(m_dimCountArray, qualified.m_dimCountArray)
					? false
					: true;
		}

		/// <summary>
		/// Read value corresponded this type from binary
		/// </summary>
		/// <param name="context">variable for making a value</param>
		/// <returns>value</returns>
		/// <seealso cref="IBlendType.ReadValue"/>
		public virtual BlendValueCapsule ReadValue(ReadValueContext context)
		{
			object obj = null;
			switch (ArrayDimension)
			{
				case 1:
					{
						var objs = new BlendValueCapsule[m_dimCountArray[0]];
						for (int i = 0; i < m_dimCountArray[0]; ++i)
						{
							objs[i] = m_baseType.ReadValue(context);
						}
						obj = objs;
					}
					break;
				case 2:
					{
						var objs = new object[m_dimCountArray[0]];

						for (int i = 0; i < m_dimCountArray[0]; ++i)
						{
							var tmp = new BlendValueCapsule[m_dimCountArray[1]];
							for (int j = 0; j < m_dimCountArray[1]; ++j)
							{
								tmp[j] = m_baseType.ReadValue(context);
							}

							objs[i] = tmp;
						}
						obj = objs;
					}
					break;
			}

			return new _BlendValueCapsule(this, obj);
		}

		public static BlendValueCapsule GetAt(BlendValueCapsule value, int index1)
		{
			Debug.Assert(value.Type.GetType() == typeof(BlendArrayType), "tyep unmatched");
			var rawValue = value.GetRawValue() as BlendValueCapsule[];
			return rawValue[index1];
		}

		public static BlendValueCapsule GetAt(BlendValueCapsule value, int index1, int index2)
		{
			Debug.Assert(value.Type.GetType() == typeof(BlendArrayType), "tyep unmatched");
			var rawValue1 = value.GetRawValue() as object[];
			var rawValue2 = rawValue1[index1] as BlendValueCapsule[];
			return rawValue2[index2];
		}

		public static IEnumerable<object> GetAllRawValue(BlendValueCapsule value)
		{
			Debug.Assert(value.Type.GetType() == typeof(BlendArrayType), "tyep unmatched");
			var type = (BlendArrayType)value.Type;

			if (type.ArrayDimension == 1)
			{
				if (type.BaseType.Equals(BlendPrimitiveType.Char()))
				{
					// Parse as string
					var objs = value.GetRawValue<BlendValueCapsule[]>();
					yield return ConvertUtil.CharArray2String(objs.Select(o => o.GetRawValue()));
				}
				else
				{
					// Parse as 1 dimension array
					var objs = value.GetRawValue<BlendValueCapsule[]>();
					foreach (var obj in objs.SelectMany(v => v.GetAllValue()))
					{
						yield return obj;
					}
				}
			}
			else if (type.ArrayDimension == 2)
			{
				// Parse as 2 dimension array
				var objs1 = value.GetRawValue<object[]>();
				foreach (BlendValueCapsule[] objs2 in objs1)
				{
					foreach (var obj in objs2.SelectMany(v => v.GetAllValue()))
					{
						yield return obj;
					}
				}
			}
			else
			{
				Debug.Assert(false, "logic error");
			}
			
		}

		
		/// <summary>
		/// get a length of array
		/// </summary>
		/// <param name="dimensionIndex">dimension</param>
		/// <returns>length</returns>
		public int GetLength(int dimensionIndex)
		{
			return m_dimCountArray[dimensionIndex];
		}

		#region private types

		private class _BlendValueCapsule : BlendValueCapsule
		{
			public _BlendValueCapsule(IBlendType type, object value) : base(type, value) { }

			override public BlendValueCapsule GetAt(int index)
			{
				return BlendArrayType.GetAt(this, index);
			}

			override public BlendValueCapsule GetAt(int index1, int index2)
			{
				return BlendArrayType.GetAt(this, index1, index2);
			}

			override public IEnumerable<object> GetAllValue()
			{
				return BlendArrayType.GetAllRawValue(this);
			}
		}

		#endregion // private types

		#region private members

		int[] m_dimCountArray;

		#endregion // private members

		#region private methods

		private bool _Equals(int[] array1, int[] array2)
		{
			if (array1 != null && array2 != null)
			{
				return array1.Equals(array2);
			}
			else
			{
				return (array1 == null && array2 == null);
			}
		}

		#endregion // private methods
	}
}

/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code is made available under the terms of the MIT License.
* https://github.com/TomasJohansson/adapters-shortest-paths/blob/master/adapters-shortest-paths-core/License.txt
*/
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static com.programmerare.shortestpaths.core.impl.WeightImpl; // SMALL_DELTA_VALUE_FOR_WEIGHT_COMPARISONS
//import static com.programmerare.shortestpaths.core.impl.WeightImpl.createWeight;
//import static org.junit.Assert.assertEquals;
//import static org.junit.Assert.assertTrue;
//import org.junit.Before;
//import org.junit.Test;
using com.programmerare.shortestpaths.core.api;

namespace com.programmerare.shortestpaths.core.impl
{
    /**
     * @author Tomas Johansson
     */
    [TestFixture]
    public class WeightImplTest {

	    private Weight weightA;
	    private Weight weightB;
	    private double weightValueA;
	    private double weightValueB;
	
	
	    [SetUp]
	    public void setUp()  {
		    weightValueA = 12345.6789;
		    weightValueB = 12345.6789;
		    weightA = createWeight(weightValueA);
		    weightB = createWeight(weightValueB);
	    }
	
	    [Test]
	    public void testGetWeightValue() {
		    AreEqual(
			    weightValueA, 
			    weightA.getWeightValue(), 
			    SMALL_DELTA_VALUE_FOR_WEIGHT_COMPARISONS
		    );
	    }
	
	    [Test]
	    public void testEquals() {
		    AreEqual(weightA, weightB);

		    IsTrue(weightA.Equals(weightB));
		    IsTrue(weightB.Equals(weightA));
	    }
	
	    [Test]
	    public void testHashCode() {
		    AreEqual(weightA.GetHashCode(), weightB.GetHashCode());
	    }	

    }
}
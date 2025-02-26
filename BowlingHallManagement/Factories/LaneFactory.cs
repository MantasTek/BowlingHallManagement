using BowlingHallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingHallManagement.Factories
{
    /*Factory Pattern Implementation for Lane class*/
    public class LaneFactory
    {
        /// <summary>
        /// Creates a new Lane with the specified lane number
        /// </summary>
        /// <param name="laneNumber">The number to assign to the lane</param>
        /// <returns>A new Lane object</returns>
        public static Lane CreateLane(int laneNumber)
        {
            // Validate lane number
            if (laneNumber <= 0)
            {
                throw new ArgumentException("Lane number must be positive", nameof(laneNumber));
            }
            
            // Create the lane
            return new Lane(laneNumber);
        }
        
        /// <summary>
        /// Creates a batch of sequential lanes starting from lane 1
        /// </summary>
        /// <param name="count">Number of lanes to create</param>
        /// <returns>An array of Lane objects</returns>
        public static Lane[] CreateLanes(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Lane count must be positive", nameof(count));
            }
            
            Lane[] lanes = new Lane[count];
            for (int i = 0; i < count; i++)
            {
                lanes[i] = CreateLane(i + 1); // Lane numbers start at 1
            }
            
            return lanes;
        }
    }
}

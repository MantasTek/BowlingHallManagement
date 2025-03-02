using BowlingHallManagement.Models;

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
            if (laneNumber <= 0)
            {
                throw new ArgumentException("Lane number must be positive", nameof(laneNumber));
            }
            
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

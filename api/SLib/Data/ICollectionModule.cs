using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SLib.Data
{
    /// <summary>
    ///   Contains methods for sequencing (ordering) a smallish collection of elements based on some property that determines the
    ///   sequence of the elements in the collection.
    /// </summary>
    /// <remarks>
    ///   The following constraints / assumptions are made by this library:
    ///     1. efficient for small (less than 1000 elements) collections.
    ///     2. user cannot maintain sequence numbering.
    ///     3. sequence numbers start at 1, and will never be 0 or negative.
    ///     4. no gaps are allowed in the sequence numbering.
    ///     5. the collection to sequence does not need to be sorted / ordered.
    /// 
    ///   A SEQUENCE is a collection of elements that have some integer property that can be used to determine the order of
    ///   the elements in the collection.
    /// </remarks>
    public class Sequencer<T>
    {
        Lens<T, int> _sequenceNo;


        public Sequencer(Lens<T, int> sequenceNo)
        {
            _sequenceNo = sequenceNo;
        }


        /// <summary>
        ///   Inserts an element into the sequence at the sequenceNo defined on the element, re-sequencing the rest of the elements.  
        ///   This method modifies the supplied sequence in place.
        /// </summary>
        public void InsertElem(ICollection<T> sequence, T elem)
        {
            Contract.Requires( sequence != null );
            Contract.Requires( elem != null );
            Contract.Requires( _sequenceNo.Get( elem ) >= 1 );
            Contract.Requires( SequenceNumbersHaveNoGaps( sequence ) );
        
            AdjustSequenceNosBelowBy( sequence, _sequenceNo.Get( elem ) - 1, 1 );
                
            sequence.Add( elem );
        }
    
    
        /// <summary>
        ///   Removes an element from the sequence at the supplied sequenceNo, re-sequencing the rest of the elements.
        ///   This method modifies the supplied sequence in place.
        /// </summary>
        public void RemoveElem(ICollection<T> sequence, int sequenceNo)
        {
            Contract.Requires( sequence != null );
            Contract.Requires( sequenceNo >= 1 );
            Contract.Requires( SequenceNumbersHaveNoGaps( sequence ) );
        
            var elemToRemove = sequence.Single( elem => _sequenceNo.Get( elem ) == sequenceNo );
        
            AdjustSequenceNosBelowBy( sequence, sequenceNo, -1 );
        
            sequence.Remove( elemToRemove );
        }
    
    
        /// <summary>
        ///   Moves an element from 1 sequenceNo to another, by just updating the sequenceNo property of the elements.  
        ///   This method does not modify the collection itself, just the elements in the collection.
        /// </summary>
        public void MoveElem(IEnumerable<T> sequence, int fromSequenceNo, int toSequenceNo)
        {
            Contract.Requires( sequence != null && sequence.Any() );
            Contract.Requires( fromSequenceNo >= 1 );
            Contract.Requires( toSequenceNo >= 1 );
            Contract.Requires( SequenceNumbersHaveNoGaps( sequence ) );
        
            // if the from & to-sequence numbers are the same, we do not need
            // to do anything
            if (fromSequenceNo == toSequenceNo)
                return;
        
            var maxSequenceNo = sequence.Max( elem => _sequenceNo.Get( elem ) );
        
            if (fromSequenceNo > maxSequenceNo)
                throw new ArgumentOutOfRangeException(nameof(fromSequenceNo), "The fromSequenceNo does not exist in the sequence.");
        
            // if to-sequence number is supplied that is greater than the max sequence
            // number, we make the assumption that the element should be placed last.
            if (toSequenceNo > maxSequenceNo)
                toSequenceNo = maxSequenceNo;
        
            var elemToMove = sequence.Single( elem => _sequenceNo.Get( elem ) == fromSequenceNo );
        
            // "removing" element from the sequence
            _sequenceNo.Set( elemToMove, -1 );
            AdjustSequenceNosBelowBy( sequence, fromSequenceNo, -1 );
        
            // "adding" element back into the seuqence at specified sequenceNo
            AdjustSequenceNosBelowBy( sequence, toSequenceNo - 1, 1 );
            _sequenceNo.Set( elemToMove, toSequenceNo );
        }
    
    
        void AdjustSequenceNosBelowBy(IEnumerable<T> sequence, int sequenceNo, int adjustment)
        {
            sequence.Where( elem => _sequenceNo.Get( elem ) > sequenceNo).ToList()
                    .ForEach( elem => _sequenceNo.Set( elem, _sequenceNo.Get( elem ) + adjustment ));
        }
    
    
        bool SequenceNumbersHaveNoGaps(IEnumerable<T> sequence)
        {
            return true;
        }
    }
}

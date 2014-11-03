using SuperMassive;
using System;

namespace NameCheck.WebApi
{
    public class BalancedPartitionKeyResolver
    {
        /// <summary>
        /// The default partition key prefix
        /// </summary>
        public const string DefaultPrefix = "";

        /// <summary>
        /// The default number of partition shards
        /// Used to distribute the users evently accross this number of partitions
        /// </summary>
        public const int DefaultNumberOfBuckets = 100;

        private int _numberOfBuckets;
        private string _partitionPrefix;

        /// <summary>
        /// Create a new instance of the <see cref="BalancedPartitionKeyResolver"/> class.
        /// </summary>
        public BalancedPartitionKeyResolver()
            : this(DefaultNumberOfBuckets, DefaultPrefix)
        { }

        /// <summary>
        /// Create a new instance of the <see cref="BalancedPartitionKeyResolver"/> class.
        /// </summary>
        /// <param name="numberOfBuckets"></param>
        public BalancedPartitionKeyResolver(int numberOfBuckets)
            : this(numberOfBuckets, DefaultPrefix)
        { }

        /// <summary>
        /// Create a new instance of the <see cref="BalancedPartitionKeyResolver"/> class.
        /// </summary>
        /// <param name="partitionPrefix"></param>
        public BalancedPartitionKeyResolver(string partitionPrefix)
            : this(DefaultNumberOfBuckets, partitionPrefix)
        { }

        /// <summary>
        /// Create a new instance of the <see cref="BalancedPartitionKeyResolver"/> class.
        /// </summary>
        /// <param name="partitionPrefix"></param>
        public BalancedPartitionKeyResolver(int numberOfBuckets, string partitionPrefix)
        {
            if (numberOfBuckets <= 0)
                throw new ArgumentException("Number of buckets is out of range", "numberOfBuckets");
            _numberOfBuckets = numberOfBuckets;
            _partitionPrefix = partitionPrefix;
        }

        /// <summary>
        /// Resolve a user id into a partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public string Resolve(string entityId)
        {
            Guard.ArgumentNotNullOrWhiteSpace(entityId, "entityId");

            return ApplyFormat(String.Format("{0}{1}{2}",
                ApplyFormat(_partitionPrefix),
                String.IsNullOrWhiteSpace(_partitionPrefix) ? "" : "_",
                (Math.Abs(ComputeHashCode(ApplyFormat(entityId))) % _numberOfBuckets) + 1));
        }

        private static string ApplyFormat(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return value;
            return value.ToLowerInvariant();
        }

        private static int ComputeHashCode(string value)
        {
            Guard.ArgumentNotNullOrWhiteSpace(value, "value");

            return BitConverter.ToInt32(CryptographyHelper.ComputeCRC32HashByte(value), 0);
        }
    }
}
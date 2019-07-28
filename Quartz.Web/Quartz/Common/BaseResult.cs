namespace Quartz.Job.Common
{
    public class BaseResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult"/> class.
        /// </summary>
        public BaseResult()
        {
            this.Code = 200;
        }

        public int Code { get; set; }

        public string Msg { get; set; }
    }
}

namespace Lenneth.WebApi.Models.Mapper
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 索引
        /// </summary>
        public long Uiid { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 用户生日
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 用户地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 用户省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 用户城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 姓名拼音
        /// </summary>
        public string Spell { get; set; }
        /// <summary>
        /// 默认银行卡
        /// </summary>
        public string BankCard { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdType { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Money { get; set; }
    }
}
using Nepy.Core;
using System;

namespace Nepy.Parsers
{
    /// <summary>
    /// 动词解析器
    /// </summary>
    /// <remarks>
    /// http://pinyin.info/readings/xinhua_pinxie_cidian/b04_verbs.html
    /// </remarks>
    [ParserIgnore]
    //[ParserOrder(14)]
    public class VerbParser:IParser
    {
        char[] chnVerbs ={   '吃', '睡', '揍', '打', '跳', '唱', '画', '说', '碰', '吹', '吸', '烤', '飞', '蹦', 
                            '站', '坐', '买', '卖', '收', '交', '叫', '喊', '听', '做', '建', '造', '修', '骂', 
                            '问', '喝', '写', '携', '歇', '卸', '放', '喊', '烧', '诊', '挣', '争', '蒸', '煮', 
                            '逐', '追', '玩', '斗', '拉', '撒', '洒', '喷', '吐', '开', '关', '管', '观', '关', 
                            '挂', '刮', '指', '值', '治', '扎', '炸', '眨', '榨', '轧', '诈', '加', '减', '架', 
                            '驾', '夹', '背', '提', '踢', '剃', '啼', '剔', '顶', '盯', '看', '漂', '飘', '瞟', 
                            '描', '瞄', '穿', '传', '脱', '扔', '偷', '投', '乘', '弃', '骑', '走', '绷', '迸', 
                            '奔', '闹', '吵', '炒', '抄', '借', '解', '洗', '系', '教', '浇', '搅', '厥', '掘', 
                            '嚼', '倒', '切', '砍', '挖', '冲', '孵', '赴', '付', '迎', '赢', '输', '读', '念', 
                            '背', '聊', '查', '插', '察', '罚', '伐', '包', '报', '抱', '撞', '推', '拿'
                        };
        public VerbParser(ParserContext context)
        {
            this.context = context;
        }
        ParserContext context;
        public ParserContext Context
        {
            get { return this.context; }
        }

        public ParseResultCollection Parse(int startIndex)
        {
            throw new NotImplementedException();
        }
    }
}

using Nepy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Parsers
{
    [ParserOrder(5)]
    public class SimChineseQuantityParser:IParser
    {
        char[] chnQuantity ={
            '把','本','笔','部','场','册','出','串','床','层','次',
            '簇','道','滴','点','顶','栋','堵','段','对','双','队',
            '顿','朵','发','番','方','份','峰','幅','副','服','杆',
            '个','根','股','挂','行','户','伙','级','剂','家','架',
            '间','件','节','句','具','棵','孔','口','块','手','打',
            '辆','列','缕','枚','门','面','名','排','匹','批','扎',
            '篇','片','撇','阕','扇','声','乘','首','束','双','丝',
            '艘','船','艇','所','台','滩','堂','套','条','头','团',
            '项','样','则','盏','张','帧','阵','只','支','枝','株',
            '桌','尊','宗','座','墙','群','客','袭','壳','贴','针',
            '线','曲','单','脚','跳','课','页','位','身','出','刀',

            '丝','毫','厘','分','钱','两','斤','担','铢','石','钧','锱','忽', '克','吨','磅',        //重量
            '毫','厘','分','寸','尺','丈','里','寻','常','铺','程','米', '码',   //长度
            '年','月','日','季','刻','时','周','天','秒','分','旬','纪','岁','世','更','夜','春','夏','秋','冬','代','伏','辈',    //时间
            '丸','泡','粒','颗','幢','堆',    //形状
            '撮','勺','合','升','斗','石','盘','碗','碟','叠','桶','笼','盆','盒','杯','钟','桩','筐','筒','篓','缸','听','包', 
            '斛','锅','簋','篮','罐','瓶','壶','卮','盏','箩','箱','煲','啖','袋','钵',    //容积
            '元','亩','毛','角','顷','文','兆', 
            '下','世','任','伙','例','具','剑','匙','卷','叶','员','回','圈',
            '地','坪','处','室','封','局','届','幕','度','式','成','截','折','拍',
            '招','拨','拳','指','掌','族','期','枪','柄','柜','栏','格','案','档',
            '梯','款','步','池','洲','派','炮','版','环','班','瓣','相','眼','窝',
            '站','章','类','组','维','起','趟','车','轮','集','尾','席','担','挑',
            '楼','村','区',   //可能与地址识别冲突
        };


        ParserContext context;
        public SimChineseQuantityParser(ParserContext context)
        {
            this.context = context;
            //this._text = NumeralUtil.ConvertChineseNumeral2Arabic(text);    //TODO::考虑在NEParser中一次性转换所有数字
        }

        public ParserContext Context
        {
            get { return this.context; }
        }

        #region IParser Members

        bool IsChineseQuantity(char ch)
        {
            for (int i = 0; i < chnQuantity.Length; i++)
            {
                if (ch == chnQuantity[i])
                    return true;
            }
            return false;
        }

        public ParseResultCollection Parse(int startIndex)
        {
            string _text = context.Text;
            ParseResultCollection prc = new ParseResultCollection();
            
            int i=startIndex;
            char ch=_text[i];
            while((NumeralUtil.IsArabicNumeral(ch)||NumeralUtil.IsChineseNumeralChars(ch)||ch=='.')
                && i+1<_text.Length )
            {
                if (i == startIndex && (NumeralUtil.IsChineseGenDigit(ch) && ch != '十'))   //首字出现进位符
                    return prc;
                ch = _text[++i];
            }
            if (i == startIndex)
                return prc;
            int j = Math.Min(i, _text.Length);
            if(IsChineseQuantity(_text[j]))
            {
                prc.Add(ParseResult.Create(_text.Substring(startIndex,i-startIndex), startIndex, POSType.A_M));
                prc.Add(ParseResult.Create(_text[i].ToString(),i,POSType.A_Q));
            }
            return prc;
        }

        #endregion
    }
}

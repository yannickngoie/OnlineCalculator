using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineCalculator
{
    public class Execute
    {

        public static string GetResult(string str)
        {
            List<char> symbleList = new List<char>();
            char[] charSymble = { '+', '-', '*', '/' };
            string[] st = str.Split(charSymble);
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '+' || str[i] == '-' || str[i] == '*' || str[i] == '/')
                {
                    symbleList.Add(str[i]);
                }
            }
            double result = Convert.ToDouble(st[0]);
            for (int i = 1; i < st.Length; i++)
            {
                double num = Convert.ToDouble(st[i]);
                int j = i - 1;
                switch (symbleList[j])
                {
                    case '+':
                        result = result + num;
                        break;
                    case '-':
                        result = result - num;
                        break;
                    case '*':
                        result = result * num;
                        break;
                    case '/':
                        result = result / num;
                        break;
                    default:
                        result = 0.0;
                        break;
                }
            }

            return result.ToString();
        }

        public static string CalcHistoryStringBuilder(string newInfo, string calcHistory)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(calcHistory))
            {
                sb.Append("<li>");
                sb.Append(newInfo);
                sb.Append("</li>");
            }

            else
            {

                sb.Append(calcHistory);
                sb.Append("<li>");
                sb.Append(newInfo);
                sb.Append("</li>");
            }

            return sb.ToString();
        }


        public static readonly Func<string, string, Func<bool>, string,string, ServiceResponse> HasValidInput = (inputValue, memoryIndicator, hasMemoryInfo, memoryValue, calcHistory) =>
        {

            char[] charInputValueArray = inputValue.ToCharArray();
            var response = new ServiceResponse();


            if (string.IsNullOrEmpty(inputValue))
            {
                response.Result = false;
                var message = MessageType.NotInputValue.ToString(); ;
            }

            else if (charInputValueArray[charInputValueArray.Length - 1].ToString() == "+" || charInputValueArray[charInputValueArray.Length - 1].ToString() == "-" || charInputValueArray[charInputValueArray.Length - 1].ToString() == "*" || charInputValueArray[charInputValueArray.Length - 1].ToString() == "/")
            {
                double currentMemoryData = 0;
                if (hasMemoryInfo())
                {
                    currentMemoryData = Convert.ToDouble(memoryValue);
                }
          
                var message = MessageType.MissingValue.ToString();
                response = MapObject(false, message, inputValue, currentMemoryData, 0, calcHistory);
            }


            else if (memoryIndicator.Contains("+"))
            {
                if (hasMemoryInfo())
                {
                    var currentInputValue = Convert.ToDouble(inputValue);
                    var currentMemoryData = Convert.ToDouble(memoryValue);
                    var memoryResult = currentMemoryData + currentInputValue;
                  response =  MapObject(true, MessageType.Success.ToString(), currentInputValue.ToString(), memoryResult, currentInputValue, calcHistory);
                }
                else
                {
                  
                    var currentMemoryData = Convert.ToDouble(inputValue);
                    response = MapObject(true, MessageType.Success.ToString(), inputValue, currentMemoryData, Convert.ToDouble(inputValue), calcHistory);
                }
                 
                             
                
            }
            else if (memoryIndicator.Contains("-"))
            {
                if (hasMemoryInfo())
                {
                    var currentInputValue = Convert.ToDouble(inputValue);
                    var currentMemoryData = Convert.ToDouble(memoryValue);
                    var memoryResult = currentMemoryData -  currentInputValue;
                    response = MapObject(true, MessageType.Success.ToString(), currentInputValue.ToString(), memoryResult, currentInputValue, calcHistory);
                }
                else
                {

                    var currentMemoryData = Convert.ToDouble(inputValue);

                    response = MapObject(true, MessageType.Success.ToString(), inputValue, currentMemoryData, Convert.ToDouble(inputValue), calcHistory);
                }

            }

            else
            {
                var _data = GetResult(inputValue);
                var newInfo = inputValue + " = " + _data;
                var history = CalcHistoryStringBuilder(newInfo, calcHistory);
                var memoryData = hasMemoryInfo() ? memoryValue : "0";
               response = MapObject(true, MessageType.Success.ToString(), inputValue, Convert.ToDouble(memoryData), Convert.ToDouble(_data), history);
            
            }

            return response;
        };


        public static ServiceResponse MapObject(bool result, string message, string currentInputValue, double currentMemoryData, double equationResult,string calcHistory)
        {
            return new ServiceResponse { Result = result, Message = message, CurrentInputValue = currentInputValue, MemoryResult = currentMemoryData, EquationResult = equationResult, CalcHistory = calcHistory };
        }

    }
    public class ServiceResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string CurrentInputValue { get; set; }
        public double? MemoryResult { get; set; }
        public string CalcHistory { get; set; }
        public double? EquationResult { get; set; }

    }
    public enum MessageType
    {
        NotInputValue,
        MissingValue,
        Success

    }

}

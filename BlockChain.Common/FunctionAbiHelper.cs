using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nethereum.ABI.Model;
//using Nethereum.Generators.Model;

namespace BlockChain.Common
{

    //public static  class FunctionAbiHelper
    //{

    //    public static FunctionABI BuildFunction(IDictionary<string, object> function)
    //    {
    //        var functionABI = new FunctionABI((string)function["name"], (bool)function["constant"],
    //            false);
    //        functionABI.InputParameters = BuildFunctionParameters((List<object>)function["inputs"]);
    //        functionABI.OutputParameters = BuildFunctionParameters((List<object>)function["outputs"]);
    //        return functionABI;
    //    }

    //    public static ParameterABI[] BuildFunctionParameters(List<object> inputs)
    //    {
    //        var parameters = new List<ParameterABI>();
    //        var parameterOrder = 0;
    //        foreach (IDictionary<string, object> input in inputs)
    //        {
    //            parameterOrder = parameterOrder + 1;
    //            var parameter = new ParameterABI((string)input["type"], (string)input["name"], parameterOrder);
    //            parameters.Add(parameter);
    //        }

    //        return parameters.ToArray();
    //    }

    //    public static FunctionABI? DeserialiseFunctionABI(string abi)
    //    {
    //        var convertor = new ExpandoObjectConverter();
    //        var function = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(abi, convertor);

    //        foreach (IDictionary<string, object> element in function)
    //        {
    //            if ((string)element["type"] == "function")
    //                return BuildFunction(element);
    //        }
    //        return null;
    //    }


    //}


    public class ABIDeserialiser
    {
        public ConstructorABI BuildConstructor(IDictionary<string, object> constructor)
        {
            var constructorABI = new ConstructorABI();
            constructorABI.InputParameters = BuildFunctionParameters((List<object>)constructor["inputs"]);
            return constructorABI;
        }

        public EventABI BuildEvent(IDictionary<string, object> eventobject)
        {
            var eventABI = new EventABI((string)eventobject["name"]);
            eventABI.InputParameters = BuildEventParameters((List<object>)eventobject["inputs"]);

            return eventABI;
        }

        public ParameterABI[] BuildEventParameters(List<object> inputs)
        {
            var parameters = new List<ParameterABI>();
            var parameterOrder = 0;
            foreach (IDictionary<string, object> input in inputs)
            {
                parameterOrder = parameterOrder + 1;
                var parameter = new ParameterABI((string)input["type"], (string)input["name"], parameterOrder)
                {
                    Indexed = (bool)input["indexed"]
                };


                parameters.Add(parameter);
            }

            return parameters.ToArray();
        }

        public FunctionABI BuildFunction(IDictionary<string, object> function)
        {
            var functionABI = new FunctionABI((string)function["name"], (bool)function["constant"],
                false);
            functionABI.InputParameters = BuildFunctionParameters((List<object>)function["inputs"]);
            functionABI.OutputParameters = BuildFunctionParameters((List<object>)function["outputs"]);
            return functionABI;
        }

        public ParameterABI[] BuildFunctionParameters(List<object> inputs)
        {
            var parameters = new List<ParameterABI>();
            var parameterOrder = 0;
            foreach (IDictionary<string, object> input in inputs)
            {
                parameterOrder = parameterOrder + 1;
                var parameter = new ParameterABI((string)input["type"], (string)input["name"], parameterOrder);
                parameters.Add(parameter);
            }

            return parameters.ToArray();
        }

        /// <summary>
        /// 得到一个合约的所有 function ，只需要使用这一个就可以了，也就是前面两行代码
        /// </summary>
        /// <param name="abi"></param>
        /// <returns></returns>
        public List<Nethereum.Generators.Model.FunctionABI> DeserialiseContract(string abi)
        {

            //Nethereum.ABI.Model.Parameter

            var f = new Nethereum.Generators.Net.GeneratorModelABIDeserialiser();
            var result = f.DeserialiseABI(abi);

            //Nethereum.Generators.Model.ParameterABI

            //var convertor = new ExpandoObjectConverter();
            //var contract = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(abi, convertor);
            var functions = new List<Nethereum.Generators.Model.FunctionABI>();
            //var events = new List<EventABI>();
            //ConstructorABI constructor = null;

            foreach (Nethereum.Generators.Model.FunctionABI element in result.Functions)
            {
                functions.Add(element);
                continue;
            }

            //if ((string)element["type"] == "function")
            //    functions.Add(BuildFunction(element));
            //if ((string)element["type"] == "event")
            //    events.Add(BuildEvent(element));
            //if ((string)element["type"] == "constructor")
            //    constructor = BuildConstructor(element);
        

            //var contractABI = new ContractABI();
            //contractABI.Functions = functions.ToArray();
            //contractABI.Constructor = constructor;
            //contractABI.Events = events.ToArray();

            return functions;
            }
        
    }

    /// <summary>
    ///     This is a replication (copy) of Newtonsoft ExpandoObjectConverter to allow for PCL compilaton
    /// </summary>
    public class ExpandoObjectConverter : JsonConverter
    {
        /// <summary>
        ///     Gets a value indicating whether this <see cref="JsonConverter" /> can write JSON.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this <see cref="JsonConverter" /> can write JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        ///     Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, object>);
        }

        /// <summary>
        ///     Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return ReadValue(reader);
        }

        /// <summary>
        ///     Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // can write is set to false
        }

        private bool IsPrimitiveToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Undefined:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return true;
                default:
                    return false;
            }
        }

        private object ReadList(JsonReader reader)
        {
            IList<object> list = new List<object>();

            while (reader.Read())
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;
                    default:
                        var v = ReadValue(reader);

                        list.Add(v);
                        break;
                    case JsonToken.EndArray:
                        return list;
                }

            throw new Exception("Unexpected end.");
        }

        private object ReadObject(JsonReader reader)
        {
            IDictionary<string, object> expandoObject = new Dictionary<string, object>();

            while (reader.Read())
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        var propertyName = reader.Value.ToString();

                        if (!reader.Read())
                            throw new Exception("Unexpected end.");

                        var v = ReadValue(reader);

                        expandoObject[propertyName] = v;
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndObject:
                        return expandoObject;
                }

            throw new Exception("Unexpected end.");
        }

        private object ReadValue(JsonReader reader)
        {
            while (reader.TokenType == JsonToken.Comment)
                if (!reader.Read())
                    throw new Exception("Unexpected end.");

            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return ReadObject(reader);
                case JsonToken.StartArray:
                    return ReadList(reader);
                default:
                    if (IsPrimitiveToken(reader.TokenType))
                        return reader.Value;

                    throw new Exception("Unexpected token when converting ExpandoObject");
            }
        }
    }




    public class ParameterABI : Parameter
    {
        public string StructType { get; set; }

        public bool Indexed { get; set; }

        public ParameterABI(string type, string name = null, int order = 1, string structType = null)
            : base(name, type, order)
        {
            StructType = structType;
        }

        public ParameterABI(string type, int order)
            : this(type, null, order)
        {
        }
    }
}

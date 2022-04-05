using System.Collections.Generic;
using API.Entitites;
using System.Collections;
using System;

namespace API.Helpers
{
    //JDO - JsonDataObject принимает дессериализированнные обьекты json строки
    //EM - EntityModel Лист типов, в которые необходимо разбить JDO
    //Example of EMList: List<Type> types = new List<Type>() { typeof(Income), typeof(IncomeDetails) };

    
    // List<Type> types = new List<Type>() { typeof(Income), typeof(IncomeDetails) };
    //         var quart = new Quarterer<JSONIncome>(objData,types);
    //         var quartedLists = quart.Quarter();
    public class Quarterer<JDO>
    {
        // List<EM> firstDec =  new List<EM>();
        private List<JDO> _jdolist;
        private List<Type> _emlist;
        //private List<List<>> resultList;
        public Quarterer(List<JDO> JDOList, List<Type> EMList)
        {
            _emlist = EMList;
            _jdolist = JDOList;
        }

        public List<IList> Quarter()
        {
            var listOfLists = CreateListOfLists();
            foreach (var jdobj in _jdolist)
            {
                foreach (var prop in jdobj.GetType().GetProperties())
                {
                    foreach (var objType in listOfLists)
                    {
                        // Type itemType = objType.GetGenericArguments()[0];
                        // Console.WriteLine(objType.GetGenericArguments()[0]);
                        //var instance = Activator.CreateInstance(objType);
                    }
                    // Console.WriteLine(prop.Name);
                    // if (.GetType().GetProperty(prop.Name) is not null)
                    // {
                        
                    // }
                }
            }
            return listOfLists;
        }


        public void ToQuart(Type objType)
        {
            // var list = new List<T>();
            // foreach (var objType in _emlist)
            // {
            //     ToQuart(objType);
            // }
            // return ;
        }

        public IList createList(Type listType)
        {
            Type genericListType = typeof(List<>).MakeGenericType(listType);
            return (IList)Activator.CreateInstance(genericListType);
        }

        private List<IList> CreateListOfLists()
        {
            List<IList> listOfLists = new List<IList>();
            foreach (Type listType in _emlist)
            {
                listOfLists.Add(createList(listType));
            }

            return listOfLists;
        }
    }
}

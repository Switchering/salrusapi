using System.Collections.Generic;
using API.Entitites;
using System.Collections;
using System;
using System.Reflection;

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
        private List<List<Tuple<int,int>>> indexList;
        //private List<List<>> resultList;
        public Quarterer(List<JDO> JDOList, List<Type> EMList)
        {
            _emlist = EMList;
            _jdolist = JDOList;
        }

        public List<IList> Quarter()
        {
            int i = 0;
            List<IList> listOfLists = CreateListOfLists();
            if (_jdolist.Count() == 0) throw new ArgumentException("JDO List is empty");
            PropertyInfo[] JDOProps = _jdolist[0].GetType().GetProperties();
            foreach (var jdObj in _jdolist)
            {   
                foreach (PropertyInfo propInfo in JDOProps)
                {
                    // foreach
                    // listOfLists[0].ad
                }
                foreach (Type emType in _emlist)  
                {
                    
                    var emObj = Activator.CreateInstance(emType);
                } 
                foreach (var list in listOfLists)
                {
                    Type listEmType = list.GetType().GetGenericArguments().Single();
                    var emObj = Activator.CreateInstance(listEmType);
                    foreach (PropertyInfo emProp in emObj.GetType().GetProperties())
                    {
                        //emProp.SetValue(jdObj.Get)
                    }
                    // list.
                }
            }
            return listOfLists;
        }

        // public T GetInstance<T>(Type type)
        // {
        //     return (T)Activator.CreateInstance(Type.GetType(type));
        // }


        public void ToQuart(Type objType)
        {
            // var list = new List<T>();
            // foreach (var objType in _emlist)
            // {
            //     ToQuart(objType);
            // }
            // return ;
        }

        public IList CreateListFromType(Type listType)
        {
            Type genericListType = typeof(List<>).MakeGenericType(listType);
            return (IList)Activator.CreateInstance(genericListType);
        }

        private List<IList> CreateListOfLists()
        {
            List<IList> listOfLists = new List<IList>();
            foreach (Type listType in _emlist)
            {
                listOfLists.Add(CreateListFromType(listType));
            }

            return listOfLists;
        }



        //USELESS
        private void EnumerateIndexes()
        {
            if (indexList is not null) indexList.Clear();
            PropertyInfo[] jdoProps = _jdolist[0].GetType().GetProperties();
            indexList = new List<List<Tuple<int,int>>>();
            //For each property of JDO input
            for (int prop = 0; prop < jdoProps.Count(); prop++)
            {
                List<Tuple<int,int>> tupleList= new List<Tuple<int,int>>();
                //For each entity model(em) in _emlist 
                for (int em = 0; em < _emlist.Count(); em++)
                {
                    //For each property of entity model(ep)
                    for (int ep = 0; ep < _emlist[em].GetProperties().Count(); ep++)
                    {
                        if (_emlist[em].GetProperties()[ep].Name == jdoProps[prop].Name)
                        {
                            Tuple<int,int> tupleTemp = new Tuple<int, int>(em, ep);
                            tupleList.Add(tupleTemp);
                        }
                    }
                }
               indexList.Add(tupleList);
            }
        }
    }
}

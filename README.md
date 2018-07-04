# Sikiro.Nosql.Mongo                                         [中文](https://github.com/SkyChenSky/Sikiro.Nosql.Mongo/blob/master/README.zh-cn.md)
This is mongo repository.Base on MongoDB.Driver.It is easy to use.

## Getting Started

### Nuget

You can run the following command to install the Sikiro.Nosql.Mongo in your project。

```
PM> Install-Package Sikiro.Nosql.Mongo
```

### Connection

```c#
var mongoRepository = new MongoRepository("mongodb://10.1.20.143:27017");
```

### Defining User Entity
```c#
[Mongo("Chengongtest", "User")]
public class User : MongoEntity
{
    public string Name { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime BirthDateTime { get; set; }

    public User Son { get; set; }

    public int Sex { get; set; }

    public List<string> AddressList { get; set; }
}
```

### Add
```c#
var addresult = mongoRepository.Add(new User
{
    Name = "skychen",
    BirthDateTime = new DateTime(1991, 2, 2),
    AddressList = new List<string> { "guangdong", "guangzhou" },
    Sex = 1,
    Son = new User
    {
        Name = "xiaochenpi",
        BirthDateTime = DateTime.Now
    }
});
```

### UPDATE
Update according to the condition part field
```c#
mongoRepository.Update<User>(a => a.Id == u.Id, a => new User { AddressList = new List<string> { "guangdong", "jiangmen", "cuihuwan" } });
```

You can also update the entity field information based on the primary key 
```c#
getResult.Name = "superskychen";
mongoRepository.Update(getResult);
```

### DELETE
Delete according to the condition
```c#
mongoRepository.Delete<User>(a => a.Id == u.Id);
```

### QUERY

#### GET
Get the first data by filtering condition

```c#
var getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
```
#### TOLIST
You can also query qualified data list.
```c#
var listResult = mongoRepository.ToList<User>(a => a.Id == u.Id);
```
### PAGELIST
```c#
var listResult = mongoRepository.PageList<User>(a => a.Id == u.Id, a => a.Desc(b => b.BirthDateTime), 1, 10);
```

### Finally a complete Demo

```c#
var url = "mongodb://10.1.20.143:27017";
var mongoRepository = new MongoRepository(url);

var u = new User
{
    Name = "skychen",
    BirthDateTime = new DateTime(1991, 2, 2),
    AddressList = new List<string> { "guangdong", "guangzhou" },
    Sex = 1,
    Son = new User
    {
        Name = "xiaochenpi",
        BirthDateTime = DateTime.Now
    }
};

var addresult = mongoRepository.Add(u);

var getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
getResult.Name = "superskychen";

mongoRepository.Update(getResult);

mongoRepository.Update<User>(a => a.Id == u.Id, a => new User { AddressList = new List<string> { "guangdong", "jiangmen", "cuihuwan" } });

mongoRepository.Exists<User>(a => a.Id == u.Id);

mongoRepository.Delete<User>(a => a.Id == u.Id);
```

### Others
In addition to the above functions, there are aggregated queries.Such as Count、Sum、Exists

## End
If you have good suggestions, please feel free to mention to me.


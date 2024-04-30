# hackernews

poc for hacker news, will use HackerNews API to retrive N best stories

## How To Debug/Build  Locally

###  Prerequisuites 

You will need below to build it locally:
- VS 2022 (or close to it)
- .NET core 8 LTS 

### Run Locally

- When you compile and run, swagger should be avaiable at localhost:5099/swagger which you can use to trigger REST methods
- or you cna curl on below:
```
http://localhost:5099/Story?topX=3
```

## Build out 

Project is based on Empty ASP.NET Core API template and below features are later on added:

- AutoMapper (to map from Proxy to Model, and model from Dto)
- added test cases (integ testes + unit test to mock)


## Assumptions 

- Set best stories cache to 5 min so that we dont hit downstream server to freq (i presume they dont change stories that fast)
- Enable eviction policy on in mem cache so that it doesnt blow up (set to 1000 stories or smth)



## If Time Permits 

- Break out hackernews proejct into Core and Hosting (API) part so that later on we can change our hosting method while Core func remains hosting agnostic
- Add more tests for corner cases/validations/error handling 
- Move from inmem cache to out of procss cache (so that we dont duplicate mem storage on each process)
- add webhook option so that we can refresh best stories cache on change (as opposed to periodic set)
- add some functional libraries (for null check, optionality + exception checks using https://github.com/louthy/language-ext)
- explore using Record feature from C# 9 (for Model)
- change tests to use the same DepInjection as main ASPNET.Core
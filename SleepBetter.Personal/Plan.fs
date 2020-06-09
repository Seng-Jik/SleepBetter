module SleepBetter.Personal.Plan

let myPlan days = 
    if days<3 then 1,0
    else if days<6 then 0,50
    else if days<9 then 0,40
    else if days<12 then 0,30
    else if days<15 then 0,20
    else if days<18 then 0,10
    else if days<21 then 0,0
    else if days<24 then 23,55
    else if days<27 then 23,50
    else if days<30 then 23,45
    else if days<33 then 23,40
    else if days<36 then 23,35
    else if days<39 then 23,30
    else if days<45 then 23,25
    else if days<50 then 23,20
    else if days<55 then 23,15
    else if days<60 then 23,10
    else if days<70 then 23,5
    else 23,0

### How price change over time

1. Customer [price would increase](../Assets\Script\Pricemenu\changeprice.cs) (X \*\* 0.5) as game progress

2. Menu cost: 30

3. Dish initial cost: 100

4. Starting expected customer price: 120

5. rate of increasing for expected price: 7 \* x ^ 0.5 (anytime)

6. Expected final price: 240

### Customer propety

#### price volility

#### patience

#### walking speed

### Cusomter spawning

- Customer leaving would decrease the reputation, [which would decrease the rate of spawning](../Assets/Script/Customer/CustomerSpawner.cs)

## Chef forgeting the recipe

13.3% percent forgeting rate

## Chef waste the ingredients

- max wasting count for each ingredient: 3
- floor(1 - currentEnergy/maxEnergy _ 3) _ 3

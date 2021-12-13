# Feedback on Sorted Developer Test

Thanks for the opportunity to do this technical test.

## Points of interest
* _Checkout_ throws a error if does not have pricing for an item
  * it is unclear if this should be done when the item is added or
    when _TotalPrice_ is calculated
* initial versions had support for serialised access to the _Basket_ to support 
  concurrent access
* concurrent access support was later removed when _TotalPrice_ was added
  because continuing to support concurrent access would mean also serialising
  calculation of _TotalPrice_ since this depends on _Basket_
  * it is not clear if concurrency is a requirement
* 'optimal' pricing was implemented by recursively splitting the order of each SKU
  into sub-orders
  * size of each sub-order is the maximum number for which we have pricing info 
  * this assumes that more items are cheaper, which is a reasonable assumption
  * it is not clear if this is a requirement but it was fun!

## Further work
* factor out price calculation into a separate module and inject into _Checkout_
  * this could defer raising an error about missing SKUs to when _TotalPrice_ is calculated
  * this would also mean that _Checkout_ would not have to check for missing SKUs

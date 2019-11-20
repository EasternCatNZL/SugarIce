using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut : Product
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ChangedToProcessedForm(Equipment.ProcessMethod processMethod)
    {
        base.ChangedToProcessedForm(processMethod);

        //change the item based on own rules
        switch (processMethod)
        {
            case Equipment.ProcessMethod.Heat:
                GameObject clone = Instantiate(processedFormList[0], transform.position, transform.rotation);
                //Set the table's object to new object
                attachedTable.itemOnTable = clone;
                //hide the new object's mesh

                //destroy self
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}

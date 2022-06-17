namespace NestApp
{
    class Controller
    {
        // SETTINGS
        const float CUT_STOCK = 0.25f;

        
        List<Part> partList = new List<Part>();

    
        public void CreatePart(int quantity, string mark, float length)
        {
            for(int i = 0; i < quantity; i++)
            {
                Part p = new Part(mark, length);
                partList.Add(p);
            }
        }

        public void scoreNest(List<Beam> resultList)
        {
            float totalLength = 0.0f;
            int count = resultList.Count;
            float totalRemaining = 0.0f;
            float totalDropPercent = 0.0f;

            float totalPartLength = 0.0f;
            int totalParts = 0;

            foreach(var beam in resultList)
            {
                totalLength += beam._length;
                totalRemaining += beam._remainingLength;

                foreach(var part in beam.sourceParts)
                {
                    totalPartLength += part._length;
                    totalParts++;
                }
            }

            totalDropPercent = totalRemaining / totalLength;

            Console.WriteLine($"Beam Qty: {count},Part Qty: {totalParts}, Total Part Length: {totalPartLength}, Total Length: {totalLength}, Total Remaining: {totalRemaining}, Total Drop% {totalDropPercent:P2}");
        }

        public void Init()
        {
            CreatePart(18, "m101", 30.5f);
            CreatePart(18, "m102", 30.5f);
            CreatePart(2, "m105", 13.0f);
            CreatePart(3, "m106", 6.5f);
            CreatePart(2, "m109", 7.5f);
            CreatePart(15, "m110", 13.0f);
            CreatePart(1, "m111", 9.5f);
            CreatePart(1, "m112", 10.25f);
            CreatePart(1, "m113", 9.5f);
            CreatePart(1, "m114", 10.25f);
            CreatePart(12, "m115", 8.25f);
            CreatePart(2, "m12", 8.75f);
            CreatePart(1, "m13", 8.5f);
            CreatePart(2, "m200", 7.5f);
            CreatePart(1, "m202", 13.0f);
            CreatePart(1, "m323", 72.0f);
            CreatePart(1, "m324", 72.0f);

            List<Beam> results = calcNest2(partList);

            foreach(var beam in results)
            {
                Console.WriteLine(beam.ToString());
            }

            scoreNest(results);
            
        }

       public bool placePart(Beam beam, Part part)
        {
            bool placed = false;

            if(part._length < beam._remainingLength)
            {
                beam.sourceParts.Add(part);
                beam._remainingLength -= (part._length + CUT_STOCK);
                part._placed = true;
                placed = true;
            } 

            return placed;
        }

         public List<Beam> calcNest(List<Part> pList)
        {
            //List<Part> sortedPartsList = pList.OrderByDescending(part => part._length).ToList();
            List<Beam> purchaseList = new List<Beam>();
            Beam beam = new Beam();
            purchaseList.Add(beam);
            Beam currentBeam = purchaseList.Last();;

            foreach(var part in pList)
            {
                if(part._length > currentBeam._remainingLength)
                {
                    Beam newBeam = new Beam();
                    purchaseList.Add(newBeam);
                    currentBeam = purchaseList.Last();

                    currentBeam.sourceParts.Add(part);
                    
                    currentBeam._remainingLength -= (part._length + CUT_STOCK);
                }else{

                    currentBeam.sourceParts.Add(part);
                    currentBeam._remainingLength -= (part._length + CUT_STOCK);
                }
            }

            return purchaseList;
        }

        // Best Nest so far
        // Need to have it compare minimum part length to remaining length to skipp some checks
        public List<Beam> calcNest2(List<Part> pList)
        {
            int totalParts = pList.Count();
            int placedParts = 0;
            float minPartLength = pList.Min( part => part._length);

            // sort part list
            List<Part> sortedPartsList = pList.OrderByDescending(part => part._length).ToList();
            // init list of beams to purchase
            List<Beam> purchaseList = new List<Beam>();
            
            // add first beam and set it to the current beam
            Beam beam = new Beam();
            purchaseList.Add(beam);
            Beam currentBeam = purchaseList.Last();


            for(int i = 0; i < totalParts; i++)
            {
                if(!sortedPartsList[i]._placed)
                {
                    // try to place a part
                    if(placePart(currentBeam, sortedPartsList[i]))
                    {
                        placedParts++;
                    }
                    else
                    {
                        i--;
                
                        for(int j = i + 1; j < totalParts; j++)
                        {
                            if(!sortedPartsList[j]._placed)
                            {
                                if(placePart(currentBeam, sortedPartsList[j]))
                                {
                                    placedParts++;
                                    break;
                                }
                                if(j == totalParts - 1)
                                {
                                    Beam newBeam = new Beam();
                                    purchaseList.Add(newBeam);
                                    currentBeam = purchaseList.Last();
                                }
                            }
                        }

                    }
                }

            }

            return purchaseList;
        }
    }
}
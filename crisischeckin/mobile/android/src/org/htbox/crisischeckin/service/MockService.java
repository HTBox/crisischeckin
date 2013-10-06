package org.htbox.crisischeckin.service;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;

import org.htbox.crisischeckin.model.Crisis;

/**MockService class is a mock class that returns crises.
 * 
 * @author Judy Carlson, My Nguyen Tra
 * @version 10/05/2013
 */
public class MockService {



	@SuppressWarnings("deprecation")
	public Collection getAllCrises() {

		Collection crises = new ArrayList<Crisis>();

		crises.add(new Crisis("1st Zombie attack", new Date(2012, 12, 10),new Date(2012, 12,14), true));
		crises.add(new Crisis("Katrina 123", new Date(2012, 8, 12),new Date(2012, 8, 17), true));
		crises.add(new Crisis("Joy of Testing", new Date(2013, 1, 1),new Date(2013, 12,1), true));
		crises.add(new Crisis("Out of Lemon Pie", new Date(2013, 12, 24),new Date(2013, 12,25), true));
		crises.add(new Crisis("Hurricane", new Date(2012, 11, 10),new Date(2012, 11,13), true));


		return crises;
	}


	public Collection getCommittedCrises() {

		Collection crises = new ArrayList<Crisis>();

		crises.add(new Crisis("1st Zombie attack", new Date(2012, 12, 10),new Date(2012, 12,14), true));
		crises.add(new Crisis("Katrina 123", new Date(2012, 8, 12),new Date(2012, 8, 17), true));

		return crises;
	}
}

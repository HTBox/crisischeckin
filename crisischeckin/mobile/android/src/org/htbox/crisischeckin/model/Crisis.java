package org.htbox.crisischeckin.model;

import java.util.Date;

/**Crisis class holds information about the crisis.
 * 
 * @author Judy Carlson, My Nguyen Tra
 * @version 10/05/2013
 */
public class Crisis {
	public String name;
	public Date startDate; // mm/dd/yyyy
	public Date endDate; // mm/dd/yyyy
	public boolean active;
	
	/**Create a new Crisis with no parameters*/
	public Crisis() {
		name = null;
		startDate = null;
		endDate = null;
		active = false;
	}
	
	/**Create a new Crisis*/
	public Crisis(String name, Date startDate, Date endDate, boolean active) {
		this.name = name;
		this.startDate = startDate;
		this.endDate = endDate;
		this.active = active;
	}
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public Date getStartDate() {
		return startDate;
	}
	public void setStartDate(Date startDate) {
		this.startDate = startDate;
	}
	public Date getEndDate() {
		return endDate;
	}
	public void setEndDate(Date endDate) {
		this.endDate = endDate;
	}
	public boolean isActive() {
		return active;
	}
	public void setActive(boolean active) {
		this.active = active;
	}
	
}
